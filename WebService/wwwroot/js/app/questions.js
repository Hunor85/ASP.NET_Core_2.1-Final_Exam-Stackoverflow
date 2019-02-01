define(['jquery', 'knockout', 'postman'], function ($, ko, postman) {

    //var currentComponent = ko.observable("post-list")
    var selectedParams = ko.observable("");

    var title = "Stackinator";
    var menuItems = [
        { name: 'Home', component: 'question-list' },
        { name: 'Cloud', component: 'Cloud' },
        {name: 'My Page', component: 'userPage'}
    ];
    console.log(menuItems);
    var selectedMenu = ko.observable(menuItems[0]);
    var selectedComponent = ko.observable("question-list");
    var isActive = function (menu) {
        return selectedMenu() === menu ? "active" : "";
    };

    var changeMenu = function (menu) {
        selectedMenu(menu);
        selectedComponent(menu.component);
    };

    postman.subscribe("changeMenu", function (menuName) {
        var menu = menuItems.find(function (m) {
            return m.name === menuName;
        });
        if (menu) changeMenu(menu);
    });

    

    postman.subscribe("selectedComponent", function (data) {
        selectedParams(data.params);
        selectedComponent(data.item);
    });


    //postman.subscribe("addSearch", function (data) {
    //    selectedParams(data.params);
    //})

    
    return {
        // currentComponent,
        title,
        selectedComponent,
        selectedParams,
        menuItems,
        isActive,
        changeMenu

        // posts,
        // prev,
        //canPrev,
        // next,
        // canNext,
        // currentView,
        // currentPost,
        //showPost,
        //back,
        //hasAnswers
    };
});