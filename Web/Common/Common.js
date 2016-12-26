function showBigImage(obj) { 
    var url = $(obj)[0].src;
    url=url.replace("_small", "");
    window.open(url);
}

