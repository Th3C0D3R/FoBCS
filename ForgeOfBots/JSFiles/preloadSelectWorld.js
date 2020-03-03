fetch("https://###WorldServer###0.forgeofempires.com/start/index?action=play_now_login", {
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
    "referrer": "https://###WorldServer###0.forgeofempires.com/page/",
    "body": "json=%7B%22world_id%22%3A%22###WORLD_ID###%22%7D",
    "method": "POST",
    "mode": "cors"
}).then(res => res.json()
.then((body)=>{
    window.location = body['login_url'];
}));