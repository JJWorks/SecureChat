
    var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    var OriginalTitle = "Secure Chat";

connection.start();



$("#buttonSubmitChat").on("click", function () {
    connection.invoke("joinRoom", "RoomName", "ThisisMyUsername");
        connection.invoke("sendChat", "RoomName", $("#chatinput").val());
        $("#chatinput").val("");
        $('#chatinput').focus();
        return false;
    });

connection.on("displayMessage",  (chatmessage, verification, isYou, userName, utcTime, spriteNumber) => {
    
        var stylestoUse = null;
        var localtime = moment(utcTime + " UTC");
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
    alert("jgbh");
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
                $("<p>").append(chatmessage)
                )
            )
        );
    });

