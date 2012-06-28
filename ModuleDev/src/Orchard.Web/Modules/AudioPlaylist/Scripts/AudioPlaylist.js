
$(function () {

    var playItem = 0;

    setupDialogs("lyricslink");
    setupPlaylist();
    
    function setupDialogs(name) {
        // get the divs with the comments and issue tracking
        var dialogDivs = $(".lyricsdiv");

        // for each dialog setup the link that opens it
        dialogDivs.each(function (index) {
            var dialogDiv = $(this);
            dialogDiv.next(".lyricslink").click(function () {
                if (dialogDiv.dialog('isOpen') == true) {
                    dialogDiv.dialog('close');
                }
                else {
                    dialogDiv.dialog('open');
                }
            });
        });

        // make it a dialog
        dialogDivs.dialog({ autoOpen: false, width: 600 });
    }

    function setupPlaylist() {
        
        $("#jquery_jplayer").jPlayer({
            ready: function () {
                setupPlaylist();
                playListInit(false); // Parameter is a boolean for autoplay.
            },
            timeupdate: function (e) {
                //          jpPlayTime.text($.jPlayer.convertTime(e.jPlayer.status.currentTime));
                //          jpTotalTime.text($.jPlayer.convertTime(e.jPlayer.status.duration));
            },
            ended: function () {
                playListNext();
            },
            swfPath: "../Swf"
        });
    }

    function setupPlaylist() {
        // get the divs with the comments and issue tracking
        var playlistItems = $(".jplayer_playlist_item");

        // for each dialog setup the link that opens it
        playlistItems.each(function () {
            var playlistItem = $(this);
            playlistItem.click(function () {
                var index =playlistItem.data("index");
                if (playItem != index) {
                    playListChange(index);
                } else {
                    $("#jquery_jplayer").jPlayer("play");
                }
                $(this).blur();
                return false;
            });
        });
    }


    function playListInit(autoplay) {
        if (autoplay) {
            playListChange(playItem);
        } else {
            playListConfig(playItem);
        }
    }

    function playListConfig(index) {
        $("#jplayer_playlist_item_" + playItem).removeClass("jplayer_playlist_current").parent().removeClass("jplayer_playlist_current");

        var currentItem = $("#jplayer_playlist_item_" + index);
        currentItem.addClass("jplayer_playlist_current").parent().addClass("jplayer_playlist_current");
        playItem = index;

        $("#jquery_jplayer").jPlayer("setMedia", { mp3: currentItem.data("mp3_file"), oga: currentItem.data("ogg_file") });
    }

    function playListChange(index) {
        playListConfig(index);
        $("#jquery_jplayer").jPlayer("play");
    }

    function playListNext() {
        var index = (playItem + 1 < myPlayList.length) ? playItem + 1 : 0;
        playListChange(index);
    }


});

   

