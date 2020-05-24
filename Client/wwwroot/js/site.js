window.scrollIntoView = () => {
    document.getElementById('loadMorePostsButton').scrollIntoView({behavior: 'smooth'});
};

window.scrollToTop = () => {
    console.log('scroll to top');
    window.scrollTo(0,0);
};

window.trackWithGoogle = (ua) => {
    window.ga=window.ga||function(){(ga.q=ga.q||[]).push(arguments)};ga.l=+new Date;
    ga('create', ua, 'auto');
    ga('send', 'pageview');
};