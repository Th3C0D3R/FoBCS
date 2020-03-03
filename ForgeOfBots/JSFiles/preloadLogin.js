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
    window.location = "https://de0.forgeofempires.com/page";
});