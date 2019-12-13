window.scrollIntoView = () => {
    console.log('hey');
    document.getElementById('loadMorePostsButton').scrollIntoView({behavior: 'smooth'});
};