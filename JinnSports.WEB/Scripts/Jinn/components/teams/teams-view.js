'use strict';

var TeamsView = function(model) {
    this.teams = [];
    this.model = model;
    this.current_page = 1;
    this.records_per_page = 10;
    this.init();
};

TeamsView.prototype = {

    init: function() {
        this.model.updateData(this.records_per_page, this.records_per_page * (this.current_page - 1), this.records_per_page);
        this.createChildren();
    },

    createChildren: function() {
        // cache the document object
        //this.$container = $('.js-container');
        this.show();
        return this;
    },

    show: function () {
        this.buildTable();
        this.buildPagingNavBar();
        this.buildSelector();
        //this.changePage(1);
    },
    
    buildSelector: function() {
        var selectContainer = document.getElementsByClassName('js-select-container')[0];
        var selector = document.createElement('select');
        selector.setAttribute('class', 'select-style');
        selector.setAttribute('id', 'tableSelector');
        var proto = this;

        selector.onchange= function () {
            proto.changeDataCount();
        }

        function addOption(oList, optionName, optionValue) {
            var oOption = document.createElement("option");
            oOption.appendChild(document.createTextNode(optionName));
            oOption.setAttribute("value", optionValue);
            oList.appendChild(oOption);
        }

        addOption(selector, '10', '10');
        addOption(selector, '25', '25');
        addOption(selector, '50', '50');
        addOption(selector, '100', '100');

        selectContainer.appendChild(selector);
    },
   
    buildTable: function() {
        var tableContainer = document.getElementsByClassName('js-table-container')[0];
        var tbl = document.createElement('table');

        //tbl.setAttribute('class', 'table');
        var tbdy = document.createElement('tbody');
        this.teams = this.model.getTeams();
        console.log(this.teams);
        alert(1);

        tbl.style.width = '100%';
        tbl.setAttribute('border', '1');
        tbl.setAttribute('id', 'teamsTable');
        console.log(this.teams.length);

        for (var i = 0; i < this.teams.length; i++) {
            //var team = this.teams;
            console.log(team);
            var row = document.createElement('tr');

            for (var team_prop in team) {
                    var cell = document.createElement('td');
                    if (team.hasOwnProperty(team_prop) ) {
                            var cellText = document.createTextNode(team[team_prop]);
                        }

                    cell.appendChild(cellText);
                    row.appendChild(cell);
                }
        tbdy.appendChild(row);
    }
    tbl.appendChild(tbdy);
    tableContainer.appendChild(tbl);
},

 rebuildTable: function() {
        var table = document.getElementById ("teamsTable");
        table.remove();
        this.buildTable();
 },

buildPagingNavBar: function()
{
    var proto = this;
    var pagingContainer = document.getElementsByClassName('js-paging-container')[0];

    var pagination_ul = document.createElement('ul');
    pagination_ul.setAttribute('class', 'pagination');
    pagination_ul.setAttribute('id', 'pagingNavBar');
    var pagination_li = document.createElement('li');
    var pagination_li_a = document.createElement('a');
    pagination_li_a.setAttribute('href', '#');

    pagination_li_a.onclick = function()
    {
        proto.prevPage();
    }

    pagination_li_a.innerHTML = '&laquo';
    pagination_li.appendChild(pagination_li_a);
    pagination_ul.appendChild(pagination_li);

    for(var iter = 0; iter < this.numPages(); iter++)
    {
        pagination_li = document.createElement('li');
        pagination_li_a = document.createElement('a'); 
        pagination_li_a.innerHTML = iter + 1;
        pagination_li_a.id = iter + 1;
        pagination_li_a.onclick = function()
        {
            proto.choosePage(this.id);
        }
        pagination_li.appendChild(pagination_li_a);
        pagination_ul.appendChild(pagination_li);
    }

    pagination_li = document.createElement('li');
    pagination_li_a = document.createElement('a');
    pagination_li_a.setAttribute('href', '#');
    pagination_li_a.onclick = function()
    {
        proto.nextPage();
    }
    pagination_li_a.innerHTML = '&raquo';
    pagination_li.appendChild(pagination_li_a);
    pagination_ul.appendChild(pagination_li);

    pagingContainer.appendChild(pagination_ul);
},
    choosePage : function(page)
    {
        if (this.current_page != page) 
        {
           this.current_page = page;
           this.changePage(this.current_page);
        }
    },

    prevPage : function()
    {
        if (this.current_page > 1) 
        {
            this.current_page--;
            this.changePage(this.current_page);
        }
    },
    
    nextPage : function()
    {
        if (this.current_page < this.numPages()) 
        {
            this.current_page++;
            this.changePage(this.current_page);
        }
    },

    numPages : function()
    {
        return Math.ceil(this.teams.length / this.records_per_page);
    },  

    changePage: function(page)
    {
        // Validate page
        if (page < 1) page = 1;
        if (page > this.numPages()) page = this.numPages();

        //отрисовка данных по номеру страницы, запрос на сервер отдельная функция
        var pagingNavBar = document.getElementById('pagingNavBar');
        var pages = pagingNavBar.getElementsByTagName('a');

         for (var i = 0; i < pages.length; i++) 
         {
            if (pages[i].getAttribute('class') == 'active')
            {
               pages[i].removeAttribute('class');
            }
            if (pages[i].id == page)
            {
               pages[i].setAttribute('class','active');
            }
         }
         this.model.updateData(this.records_per_page,  this.records_per_page * (this.current_page - 1) + 1, this.records_per_page);
         this.rebuildTable();
    },

    changeDataCount: function()
    {
        var selected_index = document.getElementById('tableSelector').selectedIndex;

        if (selected_index > 0) {
            var selected_option_value = document.getElementById('tableSelector').options[selected_index].value;
        }
        else {
            alert('non selected');
        }
        
    }
};