﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Orchard.Alias.Records;
using Orchard.Data;
using Orchard.Alias.Implementation.Holder;

namespace Orchard.Alias.Implementation.Storage {
    public interface IAliasStorage : IDependency {
        void Set(string path, IDictionary<string, string> routeValues, string source);
        IDictionary<string, string> Get(string aliasPath);
        void Remove(string path);
        void RemoveBySource(string aliasSource);
        IEnumerable<Tuple<string, string, IDictionary<string, string>, string>> List();
        IEnumerable<Tuple<string, string, IDictionary<string, string>, string>> List(string sourceStartsWith);
    }

    public class AliasStorage : IAliasStorage {
        private readonly IRepository<AliasRecord> _aliasRepository;
        private readonly IRepository<ActionRecord> _actionRepository;
        private readonly IAliasHolder _aliasHolder;
        public AliasStorage(IRepository<AliasRecord> aliasRepository, IRepository<ActionRecord> actionRepository, IAliasHolder aliasHolder) {
            _aliasRepository = aliasRepository;
            _actionRepository = actionRepository;
            _aliasHolder = aliasHolder;
        }

        public void Set(string path, IDictionary<string, string> routeValues, string source) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }

            var aliasRecord = _aliasRepository.Fetch(r => r.Path == path, o => o.Asc(r => r.Id), 0, 1).FirstOrDefault();
            aliasRecord = aliasRecord ?? new AliasRecord { Path = path };

            string areaName = null;
            string controllerName = null;
            string actionName = null;
            var values = new XElement("v");
            foreach (var routeValue in routeValues.OrderBy(kv => kv.Key, StringComparer.InvariantCultureIgnoreCase)) {
                if (string.Equals(routeValue.Key, "area", StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(routeValue.Key, "area-", StringComparison.InvariantCultureIgnoreCase)) {
                    areaName = routeValue.Value;
                }
                else if (string.Equals(routeValue.Key, "controller", StringComparison.InvariantCultureIgnoreCase)) {
                    controllerName = routeValue.Value;
                }
                else if (string.Equals(routeValue.Key, "action", StringComparison.InvariantCultureIgnoreCase)) {
                    actionName = routeValue.Value;
                }
                else {
                    values.SetAttributeValue(routeValue.Key, routeValue.Value);
                }
            }

            aliasRecord.Action = _actionRepository.Fetch(
                r => r.Area == areaName && r.Controller == controllerName && r.Action == actionName,
                o => o.Asc(r => r.Id), 0, 1).FirstOrDefault();
            aliasRecord.Action = aliasRecord.Action ?? new ActionRecord { Area = areaName, Controller = controllerName, Action = actionName };
            
            aliasRecord.RouteValues = values.ToString();
            aliasRecord.Source = source;
            if (aliasRecord.Action.Id == 0 || aliasRecord.Id == 0) {
                if (aliasRecord.Action.Id == 0) {
                    _actionRepository.Create(aliasRecord.Action);
                }
                if (aliasRecord.Id == 0) {
                    _aliasRepository.Create(aliasRecord);
                }
                // Bulk updates might go wrong if we don't flush
                _aliasRepository.Flush();
            }
            // Transform and push into AliasHolder
            var dict = ToDictionary(aliasRecord);
            _aliasHolder.SetAlias(new AliasInfo { Path = dict.Item1, Area = dict.Item2, RouteValues = dict.Item3 });
        }

        public IDictionary<string, string> Get(string path) {
            return _aliasRepository
                .Fetch(r => r.Path == path, o => o.Asc(r => r.Id), 0, 1)
                .Select(ToDictionary)
                .Select(item => item.Item3)
                .SingleOrDefault();
        }

        public void Remove(string path) {

            if (path == null) {
                throw new ArgumentNullException("path");
            }

            foreach (var aliasRecord in _aliasRepository.Fetch(r => r.Path == path)) {
                _aliasRepository.Delete(aliasRecord);
                // Bulk updates might go wrong if we don't flush
                _aliasRepository.Flush();
                var dict = ToDictionary(aliasRecord);
                _aliasHolder.RemoveAlias(new AliasInfo() { Path = dict.Item1, Area = dict.Item2, RouteValues = dict.Item3 });
            }
        }

        public void RemoveBySource(string aliasSource) {
            foreach (var aliasRecord in _aliasRepository.Fetch(r => r.Source == aliasSource)) {
                _aliasRepository.Delete(aliasRecord);
                // Bulk updates might go wrong if we don't flush
                _aliasRepository.Flush();
                var dict = ToDictionary(aliasRecord);
                _aliasHolder.RemoveAlias(new AliasInfo() { Path = dict.Item1, Area = dict.Item2, RouteValues = dict.Item3 });
            }
        }

        public IEnumerable<Tuple<string, string, IDictionary<string, string>, string>> List() {
            return _aliasRepository.Table.OrderBy(a => a.Id).Select(ToDictionary).ToList();
        }

        public IEnumerable<Tuple<string, string, IDictionary<string, string>, string>> List(string sourceStartsWith) {
            return _aliasRepository.Table.Where(a => a.Source.StartsWith(sourceStartsWith)).OrderBy(a => a.Id).Select(ToDictionary).ToList();
        }

        private static Tuple<string, string, IDictionary<string, string>, string> ToDictionary(AliasRecord aliasRecord) {
            IDictionary<string, string> routeValues = new Dictionary<string, string>();
            if (aliasRecord.Action.Area != null) {
                routeValues.Add("area", aliasRecord.Action.Area);
            }
            if (aliasRecord.Action.Controller != null) {
                routeValues.Add("controller", aliasRecord.Action.Controller);
            }
            if (aliasRecord.Action.Action != null) {
                routeValues.Add("action", aliasRecord.Action.Action);
            }
            if (!string.IsNullOrEmpty(aliasRecord.RouteValues)) {
                foreach (var attr in XElement.Parse(aliasRecord.RouteValues).Attributes()) {
                    routeValues.Add(attr.Name.LocalName, attr.Value);
                }
            }
            return Tuple.Create(aliasRecord.Path, aliasRecord.Action.Area, routeValues, aliasRecord.Source);
        }
    }
}