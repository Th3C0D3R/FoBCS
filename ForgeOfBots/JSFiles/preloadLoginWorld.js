if (document.readyState == "complete") FoEInit();
else {
    document.addEventListener("DOMContentLoaded", () => {
        FoEInit();
    });
}

function FoEInit() {
    FoELogin();
}

async function FoELogin() {
    if (window.location.href.indexOf("de0") <= 0) {
        fetch("https://de.forgeofempires.com/glps/login_check", {
            "credentials": "include",
            "headers": {
                "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0",
                "Accept": "application/json, text/plain, */*",
                "Accept-Language": "de,en-US;q=0.7,en;q=0.3",
                "X-XSRF-TOKEN": "###XSRF-TOKEN###",
                "Content-type": "application/x-www-form-urlencoded; charset=UTF-8",
                "X-Requested-With": "XMLHttpRequest",
                "Pragma": "no-cache",
                "Cache-Control": "no-cache"
            },
            "referrer": "https://de.forgeofempires.com/",
            "body": "login%5Buserid%5D=###USERNAME###&login%5Bpassword%5D=###PASSWORD###&login%5Bremember_me%5D=false",
            "method": "POST",
            "mode": "cors"
        }).catch((e) => {
            console.log(e);
        }).then((x) => {
            document.location = "https://de0.forgeofempires.com/page";

            fetch("https://de0.forgeofempires.com/start/index?action=fetch_worlds_for_login_page", {
                "credentials": "include",
                "headers": {
                    "accept": "text/plain, */*; q=0.01",
                    "accept-language": "de",
                    "cache-control": "no-cache",
                    "content-type": "application/x-www-form-urlencoded; charset=UTF-8",
                    "pragma": "no-cache",
                    "sec-fetch-mode": "cors",
                    "sec-fetch-site": "same-origin",
                    "x-requested-with": "XMLHttpRequest"
                },
                "referrer": "https://de0.forgeofempires.com/page/",
                "referrerPolicy": "no-referrer-when-downgrade",
                "body": "json=null",
                "method": "POST",
                "mode": "cors"
            }).then(res => {
                res.text().then(body => {
                    //await FoETimer(1000);
                    fetch("https://de0.forgeofempires.com/start/index?action=play_now_login", {
                        "credentials": "include",
                        "headers": {
                            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0",
                            "Accept": "text/plain, */*; q=0.01",
                            "Accept-Language": "de,en-US;q=0.7,en;q=0.3",
                            "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8",
                            "X-Requested-With": "XMLHttpRequest",
                            "Pragma": "no-cache",
                            "Cache-Control": "no-cache"
                        },
                        "referrer": "https://de0.forgeofempires.com/page/",
                        "body": "json=%7B%22world_id%22%3A%22" + Object.keys(body["playable_worlds"])[0] + "%22%7D",
                        "method": "POST",
                        "mode": "cors"
                    }).then(res => res.json()
                        .then((body) => {
                            window.location = body['login_url'];
                            //await FoETimer(1000);
                        }));
                });
            });
        });
    }
    else {
        fetch("https://de0.forgeofempires.com/start/index?action=fetch_worlds_for_login_page", {
            "credentials": "include",
            "headers": {
                "accept": "text/plain, */*; q=0.01",
                "accept-language": "de",
                "cache-control": "no-cache",
                "content-type": "application/x-www-form-urlencoded; charset=UTF-8",
                "pragma": "no-cache",
                "sec-fetch-mode": "cors",
                "sec-fetch-site": "same-origin",
                "x-requested-with": "XMLHttpRequest"
            },
            "referrer": "https://de0.forgeofempires.com/page/",
            "referrerPolicy": "no-referrer-when-downgrade",
            "body": "json=null",
            "method": "POST",
            "mode": "cors"
        }).then(res => {
            res.text().then(body => {
                console.log(JSON.parse(body)["player_worlds"]);
                //await FoETimer(1000);
                fetch("https://de0.forgeofempires.com/start/index?action=play_now_login", {
                    "credentials": "include",
                    "headers": {
                        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0",
                        "Accept": "text/plain, */*; q=0.01",
                        "Accept-Language": "de,en-US;q=0.7,en;q=0.3",
                        "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8",
                        "X-Requested-With": "XMLHttpRequest",
                        "Pragma": "no-cache",
                        "Cache-Control": "no-cache"
                    },
                    "referrer": "https://de0.forgeofempires.com/page/",
                    "body": "json=%7B%22world_id%22%3A%22" + Object.keys(JSON.parse(body)["player_worlds"])[0] + "%22%7D",
                    "method": "POST",
                    "mode": "cors"
                }).then(res => res.json()
                    .then((body) => {
                        window.location = body['login_url'];
                        //await FoETimer(1000);
                    }));
            });
        });
    }
}

function FoFTimer(time) {
    if (time == void 0) time = 500;
    return new Promise((res) => {
        setTimeout(() => { res(); }, time);
    });
}