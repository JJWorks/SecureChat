


/*
 * Audio Playing
 */
function PlayAwayAudio() {
    if (!$("#speakerbutton").hasClass("speakerOff")) {
        var audio = new Audio("./Audio/Notification.mp3");
        audio.play();
    }
}

/*
 * Scroll Functionality
 * 
 */
function scrollMessageReaction() {
    var isBottom = $("#divChatMessages").scrollTop() + $("#divChatMessages").innerHeight() >= $("#divChatMessages")[0].scrollHeight;
    if (isBottom) {
        if ($("#MoreMessagesWarning").css("display") !== "none") 
            $("#MoreMessagesWarning").hide();
    }
    else {
        if ($("#MoreMessagesWarning").css("display") === "none") {
            $("#MoreMessagesWarning").show();
        }
    }
}


function setScrolltoBottom() {
    var element = document.getElementById("divChatMessages");
    element.scrollTop = element.scrollHeight;
}


/*
 * CryptoJS Encryption
 */
String.prototype.AESEncrypt = function (Key) {
    return CryptoJS.AES.encrypt(this.toString(), Key).toString();
};

String.prototype.AESDecrypt = function (Key) {
    return CryptoJS.AES.decrypt(this.toString(), Key).toString(CryptoJS.enc.Utf8);
};


/*
 * String and Character Manipulation
 */
String.prototype.replaceAll = function (target, replacement) {
    return this.split(target).join(replacement);
};

String.prototype.replaceAllNewLine = function () {
    return this.split(/\n/).join("<br />");
};

function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

function escapeHtml(str) {
    var div = document.createElement('div');
    div.appendChild(document.createTextNode(str));
    return div.innerHTML;
}


/*
 * Emoji Fun
 */
function ReplaceWithAllEmotes(TextToReplace) {

    for (var i = 0; i < EmoteData.length; i++) {
        for (var j = 0; j < EmoteData[i].codes.length; j++) {
            TextToReplace = TextToReplace.replaceWithEmotes(EmoteData[i].codes[j], EmoteData[i].imagefile, EmoteData[i].title);
        }
    }
    return TextToReplace;
}


String.prototype.replaceWithEmotes = function (target, imageName, Description) {
    return this.split(target).join("<img class=\"emote\" src=\"Emotes/" + imageName + "\" alt=\"" + Description + "\" />");
};

var EmoteData = null;
function GetEmotes() {
    if (EmoteData === null)
        $.getJSON("api/Emote").done(function (data) {
            EmoteData = data;
        });
}