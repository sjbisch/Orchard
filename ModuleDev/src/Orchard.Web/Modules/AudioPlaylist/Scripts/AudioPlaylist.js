
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
                setupPlayLinks();
                playListInit(false); // Parameter is a boolean for autoplay.
            },
            timeupdate: function (e) {
                $("#jplayer_playlist_item_" + playItem + "_current_time").text($.jPlayer.convertTime(e.jPlayer.status.currentTime));
                $("#jplayer_playlist_item_" + playItem + "_duration").text($.jPlayer.convertTime(e.jPlayer.status.duration));
                if (e.jPlayer.status.currentTime > 0) {
                    $(".jplayer_playlist_current .item_time").show();
                }                
            },
            ended: function () {
                playListNext();
            }
        });
    }

    function setupPlayLinks() {

        $("#jplayer_play").toggle(
            playAudio,
            pauseAudio
        );

        // get the links to play the songs
        var playlistItems = $(".jplayer_playlist_item");

        // for each dialog setup the link that opens it
        playlistItems.each(function () {
            var playlistItem = $(this);
            playlistItem.toggle(function () {
                var index =playlistItem.data("index");
                if (playItem != index) {
                    playListChange(index);
                } else {
                    playAudio();
                }
                $(this).blur();
                return false;
            },
            pauseAudio
            );
        });
    }
    
    function playAudio() {
        $("#jquery_jplayer").jPlayer("play");
        $("#jplayer_play").text("Pause");
    }

    function pauseAudio() {
        $("#jquery_jplayer").jPlayer("pause");
        $("#jplayer_play").text("Play");
    }


    function playListInit(autoplay) {
        if (autoplay) {
            playListChange(playItem);
        } else {
            playListConfig(playItem);
        }
    }

    function playListConfig(index) {
        $(".jplayer_playlist_current .item_time").removeAttr("style");
        $("#jplayer_playlist_item_" + playItem).removeClass("jplayer_playlist_current").parent().removeClass("jplayer_playlist_current");
        

        var currentItem = $("#jplayer_playlist_item_" + index);
        currentItem.addClass("jplayer_playlist_current").parent().addClass("jplayer_playlist_current");
        $(".jplayer_playlist_current .item_time").hide();
        playItem = index;

        $("#jquery_jplayer").jPlayer("setMedia", { mp3: currentItem.data("mp3_file"), oga: currentItem.data("ogg_file") });
    }

    function playListChange(index) {
        playListConfig(index);
        playAudio();
    }

    function playListNext() {
        var index = (playItem + 1 < myPlayList.length) ? playItem + 1 : 0;
        playListChange(index);
    }


});

   

