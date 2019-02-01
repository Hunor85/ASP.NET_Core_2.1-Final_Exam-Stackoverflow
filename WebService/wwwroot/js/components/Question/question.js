define(['knockout', 'dataService', 'postman'], function (ko, ds, postman) {

    return function (params) {

        var currentPostAnswer = ko.observable();
        var currentPostComment = ko.observable();
         
        var currentComponent = ko.observable("question");
        var hasAnswers = ko.observable(false);
        var curLink = params.link;

        var PostId = params.question.PostId;
        var UserId = params.question.User.Id;
        
        var getPostAnswers = function (url) {
            ds.getPost(url, function (data) {
                $.getJSON(data.answers, function (answers) {
                    hasAnswers(answers && answers.length > 0);
                    data.answers = answers;
                    currentPostAnswer(data);

                });
                $.getJSON(data.comments, function (comments) {
                    data.comments = comments;
                    currentPostComment(data)
                    
                });
            });
        }

       

        getPostAnswers(curLink);

        var back = function () {
           // ds.getPosts("api/questions/name/"+ backTerm + "");
            postman.publish("selectedComponent", { item: "question-list", params: {back: params.back} });

        };
        
    /*var addAnnotation = function() 
    {
            try {
                $.ajax({
                    url: '/api/annotations',
                    type: 'post',
                    dataType: 'json',
                    data: ko.toJSON(this), 
                    contentType: 'application/json',
                    body: {
                        "PostId" : 19
                        "UserId" : 2,
                        "Body" : "test"
                    }
                });
            } catch (e) {
                 console.log(e);
            }
        };*/

        var addAnnotation = (post) => {            
            ds.post("api/annotations", { PostId: PostId, UserId: UserId }, (response) => {
                console.log(response);
                bookmarkId = response.id;
            });
        }
        

        return {
            getPostAnswers,
            //getPostComments,
            currentPostAnswer,
            currentPostComment,
            hasAnswers,
            back,
            currentComponent,
            addAnnotation

       };
});