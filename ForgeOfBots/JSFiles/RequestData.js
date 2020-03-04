document.addEventListener("DOMContentLoaded",
    function () {
        (function (open) {
            XMLHttpRequest.prototype.open = function (method, url, async, user, pass) {
                this.addEventListener("readystatechange",
                    function () {
                        if (this.readyState === 4) {
                            window.jsInterface.hook(this.responseText, method, url, async, user, pass, "Context");
                        }
                    },
                    false);
                open.call(this, method, url, async, user, pass);
            };
        })(XMLHttpRequest.prototype.open);
    });