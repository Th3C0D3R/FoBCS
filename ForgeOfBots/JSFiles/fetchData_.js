var fobcallback = arguments[arguments.length - 1];
const request = async () => {
    const res = await fetch("https://##WorldID##.forgeofempires.com/game/json?h=##UserKey##", {
        "credentials": "include",
        "headers": {
            "accept": "*/*",
            "accept-language": "en-US,en;q=0.9,de-DE;q=0.8,de;q=0.7",
            "client-identification": "version=##Version##; requiredVersion=##Version##; platform=bro; platformType=html5; platformVersion=web",
            "content-type": "application/json",
            "sec-fetch-mode": "cors",
            "sec-fetch-site": "same-origin",
            "signature": "##sig##",
            "user-agent": "##UserAgent##"
        },
        "referrer": "https://##WorldID##.forgeofempires.com/game/index?",
        "referrerPolicy": "no-referrer-when-downgrade",
        "body": '##RequestData##',
        "method": "POST",
        "mode": "cors"
    });
    if (res.status != 200) return "";
    var body = await res.text();
    try {
        var json = JSON.parse(body);
        if (##onlyOne##) {
            var newBody = "";
            if ("##resType##" == "VisitTavern") {
                for (var i = 0; i < json.length; i++) {
                    newBody += "##methode##" + JSON.stringify(json[i]) + "##@##";
                }
            }
            else {
                for (var i = 0; i < json.length; i++) {
                    const resData = json[i];
                    if (resData["requestMethod"] == "##methode##") {
                        newBody += "##methode##" + JSON.stringify(json[i]) + "##@##";
                    }
                    else if (resData["requestMethod"] == "rewardResources") {
                        newBody += "rewardResources" + JSON.stringify(json[i]) + "##@##";
                    }
                }
            }
            if (newBody.indexOf("##@##") == newBody.lastIndexOf("##@##"))
                newBody = newBody.replace("##methode##", "");
            body = newBody.slice(0, newBody.lastIndexOf("##@##"));
        }
        else {
            var newBody = "";
            for (var i = 0; i < json.length; i++) {
                const resData = json[i];
                if (resData["requestMethod"] == "getData")
                    newBody += "getData" + JSON.stringify(json[i]) + "##@##";
                if (resData["requestMethod"] == "getMetadata")
                    newBody += "getMetadata" + JSON.stringify(json[i]) + "##@##";
                if (resData["requestMethod"] == "getPlayerResources")
                    newBody += "getPlayerResources" + JSON.stringify(json[i]) + "##@##";
                if (resData["requestMethod"] == "getResourceDefinitions")
                    newBody += "getResourceDefinitions" + JSON.stringify(json[i]) + "##@##";
                if (resData["requestMethod"] == "getOverview")
                    newBody += "getOverview" + JSON.stringify(json[i]) + "##@##";
                if (resData["requestMethod"] == "getOtherTavernStates")
                    newBody += "getOtherTavernStates" + JSON.stringify(json[i]) + "##@##";
                if (resData["requestMethod"] == "getSittingPlayersCount")
                    newBody += "getSittingPlayersCount" + JSON.stringify(json[i]) + "##@##";
                if (resData["requestMethod"] == "getUpdates")
                    newBody += "getUpdates" + JSON.stringify(json[i]) + "##@##";
                if (resData["requestMethod"] == "getLimitedBonuses")
                    newBody += "getLimitedBonuses" + JSON.stringify(json[i]) + "##@##";
                if (resData["requestMethod"] == "updateEntity")
                    newBody += "updateEntity" + JSON.stringify(json[i]) + "##@##";
                if (resData["requestMethod"] == "getConstruction")
                    newBody += "getConstruction" + JSON.stringify(json[i]) + "##@##";
            }
            body = newBody;
        }
        if (json[0]["__class__"] === "Error" || json[0]["__class__"] === "Redirect")
            return "SESSION-EXPIRED";
        return body;
    } catch (error) {
        alert(error);
        return JSON.stringify(error);
    }
    alert("ERROR!!");
    return "";
}

fobcallback(request());
