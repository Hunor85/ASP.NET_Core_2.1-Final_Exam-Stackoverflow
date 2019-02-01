define(['knockout', 'dataService', 'postman'], function (ko, ds, postman) {

    return function (params) {
        var posts = ko.observableArray([]);
        var currentComponent = ko.observable("question")
        var canPrev = ko.observable(false);
        var prevUrl = "";
        var canNext = ko.observable(false);
        var nextUrl = "";
        var total = 0;
        var searchVal = ko.observable(params.back);
        var postData = { search: "hello world", userId: "13" };

        //testvalues
        var id = "13"



        var getPosts = function (url) {

            ds.getPosts(url, function (data) {
                total = data.total
                curUrl = data.cur;
                prevUrl = data.prev;
                canPrev(false);
                data.prev !== null && canPrev(true);
                nextUrl = data.next;
                canNext(false);
                data.next !== null && canNext(true);
                posts(data.items);
            });
        };

        var getSearch = function (terms) {
            ds.searchPosts(terms, function (data) {
                //searchVal = data.terms
                total = data.total
                curUrl = data.cur;
                prevUrl = data.prev;
                canPrev(false);
                data.prev !== null && canPrev(true);
                nextUrl = data.next;
                canNext(false);
                data.next !== null && canNext(true);
                posts(data.items);
                console.log(data.items);

            })

        }
        if (searchVal() === undefined) searchVal("")
        else getSearch(searchVal());

        

        var next = function () {
            getPosts(nextUrl);
        };

        var prev = function () {
            getPosts(prevUrl);
        };

        var searchPost = function () {

            
            getSearch(searchVal())
            $.ajax({
                type: 'POST',
                url: 'api/searchhistory/add/',
                // The key needs to match your method's input parameter (case-sensitive).
                data: JSON.stringify({ search: searchVal(), userId: "13" }),
                contentType: 'application/json',

            });
        };

     
        var showPost = function (post) {
          
            postman.publish("selectedComponent", { item: "question", params: { link: post.link, back: searchVal() } });
        };

       

        return {
            posts,
            prev,
            canPrev,
            searchPost,
            next,
            canNext,
            searchVal,
            


            // selectComponent,
            showPost,
            currentComponent,


            //test
            id


        };

    };

});