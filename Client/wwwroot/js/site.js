window.scrollIntoView = () => {
    document.getElementById('loadMorePostsButton').scrollIntoView({behavior: 'smooth'});
};

window.scrollToTop = () => {
    console.log('scroll to top');
    window.scrollTo(0,0);
};