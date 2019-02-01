
require.config({
    baseUrl: "js",
    paths: {
        jquery: "lib/jQuery/dist/jquery.min",
        knockout: "lib/knockout/dist/knockout.debug",
        dataService: "services/ds",
        jqcloud: 'lib/jqcloud2/dist/jqcloud',
        text: "lib/text/text",
        postman: 'services/postman'
    },
    shim: {
        // set default deps
        'jqcloud': ['jquery']
    }
});


 require(['jquery', 'knockout', 'jqcloud'], function ($, ko) {
    ko.bindingHandlers.cloud = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
           
            var cloud = allBindings.get('cloud');
            var words = cloud.words;

            // if we have words that is observables
            if (words && ko.isObservable(words)) {
                // then subscribe and update the cloud on changes
                words.subscribe(function () {
                    $(element).jQCloud('update', ko.unwrap(words));
                });
            }

        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
           
            var cloud = allBindings.get('cloud');

            
            var words = ko.unwrap(cloud.words) || [];
            var width = cloud.height || 200;
            var height = cloud.height || 200;

            // to show the cloud we call the jqcloud function
            $(element).jQCloud(words, {
                width: width,
                height: height
            });
        }
    };
});
// register components
require(['knockout'], function (ko) {

    ko.components.register("question-list",
        {
            viewModel: { require: 'components/QuestionList/questionList' },
            template: { require: 'text!components/QuestionList/questionListView.html' }
        });

    ko.components.register("question",
        {
            viewModel: { require: 'components/Question/question' },
            template: { require: 'text!components/Question/questionView.html' }
        });
    ko.components.register("cloud", {
        viewModel: { require: 'components/cloud/wordCloud' },
        template: { require: 'text!components/cloud/wordCloudView.html' }
    });
    ko.components.register("userPage", {
        viewModel: { require: 'components/User/user' },
        template: { require: 'text!components/User/userView.html' }
    });
});

require(['knockout', 'app/questions', 'jqcloud'], function (ko, questionVm) {
    ko.applyBindings(questionVm);
});