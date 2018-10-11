var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").configureLogging(signalR.LogLevel.Trace).build();


var OriginalTitle = "Secure Chat Core";

/// Action to Connect to the Chat.
$("#formLogin").submit(function () {
    $("#ulChatUsers").empty();
    $("#ulChatPanel").empty();
    if (EmoteData === null)
        GetEmotes();
        
    $("#SignInContainer").hide();
    document.title = $("#inputChatRoom").val().trim() + " Secure Chat Room";
    $("nav").show("fast", function () {
        // Animation complete.
    });
    $("#divChat").show("fast", function () {
    });


    connection.start().catch(function (err) {
        return console.error(err.toString());
    }).then(value =>
        connection.invoke("JoinRoom", $("#inputChatRoom").val().trim(), $("#inputUserName").val().trim()));
        
    //gegg(aaaa);
    $("#MoreMessagesWarning").click();
    $('#chatinput').focus();
    return false;
}).done;




/// Action to Disconnect from the Chat
$('#buttonDisconnect').click(function () {
    $("nav").hide();
    $("#divChat").hide();
    $("#SignInContainer").show();
    document.title = OriginalTitle;
    connection.stop();
    //$("#chatinput").unbind();
    return false;
});

$("#buttonSubmitChat").on("click", function () {
    connection.invoke("sendChat", $("#inputChatRoom").val(), $("#chatinput").val().AESEncrypt($("#inputPassword").val()));
    $("#chatinput").val("");
    $('#chatinput').focus();
    return false;
});


$("#chatinput").keydown(function (event) {
    if (event.keyCode === 13 && !event.shiftKey && $("#divChat").css("display") !== "none") {
        $("#buttonSubmitChat").click();
    }
});

$("#chatinput").keyup(function (event) {
    if (event.keyCode === 13 && !event.shiftKey) {
        if ($("#chatinput").val().substr(0, 1) === "\n")
            $("#chatinput").val($("#chatinput").val().substr(1, $("#chatinput").val().length - 1));
    }
});

$("#divChatMessages").on("scroll", function () {
    scrollMessageReaction();
});

$("#MoreMessagesWarning").on("click", function () {
    $("#divChatMessages").animate({ scrollTop: $('#divChatMessages').prop("scrollHeight") }, 300);
    $("#MoreMessagesWarning").hide();
});

$("#speakerbutton").click(function () {
    if ($("#speakerbutton").hasClass("speakerOn")) {
        $("#speakerbutton").toggleClass("speakerOn");
        $("#speakerbutton").toggleClass("speakerOff");
    }
    else if ($("#speakerbutton").hasClass("speakerOff")) {
        $("#speakerbutton").toggleClass("speakerOn");
        $("#speakerbutton").toggleClass("speakerOff");
    }
});

connection.on("loadUsers", (inputArray) => {
    $("#ulChatUsers").empty();
    inputArray = JSON.parse(inputArray);
    $.each(inputArray, function (index, value) {
        addUser(value);
    });
});

connection.on("hubAddUser", (userObject) => {
userObject = JSON.parse(userObject);
    addUser(userObject);
});

connection.on("removeUser", function (userUniqueID) {
    $("#" + userUniqueID).remove();
});


    




var window_focus;
var newMessage_interval = null;
var ChatTitle;
var missMessagecounter = 0;



$(window).focus(function () {
    window_focus = true;
    if (missMessagecounter > 0)
        missMessagecounter = 0;
    if (newMessage_interval !== null) {
        clearInterval(newMessage_interval);
        newMessage_interval = null;
        document.title = ChatTitle;
    }
}).blur(function () {
    window_focus = false;
});





function addUser(userObject) {
    var UserDisplay;
    if (userObject.Verified) {
        UserDisplay = "<span class=\"mdi mdi-crown VerficationUserNameFont\" title=\"Verified\"></span> " + userObject.UserName;
    }
    else
        UserDisplay = userObject.UserName;
    $("#ulChatUsers").append(
        $("<li>").attr("id", userObject.Identifier).addClass("list-group-item userInfo").append(UserDisplay)
    );
}



function ValidateFormKeyMatch(input) {
    if (input.value !== $("#inputPassword").val()) {
        input.setCustomValidity("Key does not match");
    }
    else {
        try {
            input.setCustomValidity('');
        }
        catch (e) { }
    }
}


connection.on("displayMessage", (chatmessage, verification, isYou, userName, utcTime, spriteNumber) => {
    var stylestoUse = null;
    var localtime = moment(utcTime);
    var HTMLUserString;
    var VerificationHTMLUserString = !verification ? userName : "<span class=\"mdi mdi-crown VerficationUserNameFont\" title=\"Verified\"></span> " + userName;
    if (isYou) {
        stylestoUse = { liclass: "right clearfix", spanimageclass: "chat-img pull-right", userNameClass: "pull-right primary-font", smallclass: "text-muted" };
        HTMLUserString = "<small class=\"" + stylestoUse.smallclass + "\"><span class=\"glyphicon glyphicon-time\"></span>" + localtime.format('LT') + "</small><strong class=\"" + stylestoUse.userNameClass + "\">" + VerificationHTMLUserString + "</strong>";
    }
    else {
        stylestoUse = { liclass: "left clearfix", spanimageclass: "chat-img pull-left", userNameClass: "primary-font", smallclass: "pull-right text-muted" };
        HTMLUserString = "<strong class=\"" + stylestoUse.userNameClass + "\">" + VerificationHTMLUserString + "</strong><small class=\"" + stylestoUse.smallclass + "\"><span class=\"glyphicon glyphicon-time\"></span>" + localtime.format('LT') + "</small>";
    }

    var isBottom = $("#divChatMessages").scrollTop() + $("#divChatMessages").innerHeight() >= $("#divChatMessages")[0].scrollHeight;
    if (isBottom) {
        $("#divChatMessages").unbind("scroll");
    }

    var DecodedMessage = escapeHtml(chatmessage.AESDecrypt($("#inputPassword").val()));
    DecodedMessage = ReplaceWithAllEmotes(DecodedMessage);
    DecodedMessage = DecodedMessage.replaceAllNewLine();

    $("#ulChatPanel").append(
        $("<li>").addClass(stylestoUse.liclass).append(
            $("<div>").addClass(stylestoUse.spanimageclass).append(
                $("<div>").addClass("userNameSprite" + spriteNumber.toString() + " img-circle").append(
                    "<span class=\"CircleNameFont\">" + (verification ? "<span class=\"mdi mdi-crown VerficationUserNameFont\" title=\"Verified\"></span>" : "") + userName.substring(0, 1) + "</span>"
                )
            )
        ).append(
            $("<div>").addClass("chat-body clearfix").append(
                $("<div>").addClass("header").append(HTMLUserString
                )
            ).append(
                $("<p>").append(DecodedMessage)
            )
        )
    );
    if (isBottom) {
        $("#divChatMessages").animate({ scrollTop: $('#divChatMessages').prop("scrollHeight") }, 300,
            function () { $("#divChatMessages").bind("scroll", function () { scrollMessageReaction(); }); }
        );
    }

    if (!window_focus && !isYou) {
        PlayAwayAudio();
        missMessagecounter++;
        if (newMessage_interval === null)
            ChatTitle = document.title;
        var blink = function () { document.title = document.title !== ChatTitle ? ChatTitle : numberWithCommas(missMessagecounter) + " Missed Messages - " + ChatTitle; };
        if (newMessage_interval === null)
            newMessage_interval = setInterval(blink, 1000);
    }
});