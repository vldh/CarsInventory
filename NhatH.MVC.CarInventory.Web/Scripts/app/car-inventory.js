

var CarModel = function (car) {
    var self = this;
    /*custom bindingHandler for error message*/
    ko.bindingHandlers.validationCore = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            // insert the message
            var span = document.createElement('SPAN'); //element to hold error message
            span.className = 'validationMessage';      //error message style
            var parent = $(element).parent().closest
                (".input-group");       //find the holder div of the input
            if (parent.length > 0) {
                $(parent).after(span);    //has holder: add message holder just after the input holder       
            } else {
                $(element).after(span);   //no holderL add message holder just after the input itself
            }
            ko.applyBindingsToNode(span, { validationMessage: valueAccessor() });
        }
    }; 
    /*error span would not be inserted and we have to specify the location of error message*/
    ko.validation.init({ insertMessages: false });
    self.id = ko.observable();
    self.brand = ko.observable().extend({ required: { message: 'Please enter brand name' } });
    self.model = ko.observable().extend({ required: { message: 'Please enter model name' } });
    self.year = ko.observable().extend({ required: { message: 'Please enter year' }, digit: 'Please enter a number', minLength: 4 });
    self.price = ko.observable().extend({ required: { message: 'Please enter price' }, number: 'Please enter a number' });
    self.isNew = ko.observable();
    self.visible = ko.observable(true);
    self.init = function ()
    {
        if (car != null)
        {
            self.id(car.ID);
            self.brand(car.Brand);
            self.model(car.Model);
            self.year(car.Year);
            self.price(car.Price);
            self.isNew(car.IsNew);
        }
    }

    self.init();
    self.errors = ko.validation.group(self);
}

var CarManagement = function ()
{
    var self = this;
    self.cars = ko.observableArray([]);
    self.text = ko.observable().extend({ required: true });
    self.isEdit = ko.observable(false);
    self.currentCar = ko.observable(new CarModel());
    self.query = ko.observable("");
    self.filteredCars = ko.computed(function () {
        var filter = self.query().toLowerCase();

        if (!filter) {
            return self.cars();
        } else {
            return ko.utils.arrayFilter(self.cars(), function (item) {
                var b = item.brand();
                var m = item.model();
                if (b === null) {
                    b = '';
                }
                else {
                    b = b.toString().toLowerCase();
                }
                if (m === null) {
                    m = '';
                }
                else {
                    m = m.toString().toLowerCase();
                }
                return b.indexOf(filter) !== -1 || m.indexOf(filter) !== -1;
            });
        }
    });
    
    self.getCars = function () {

        $.ajax({
            url: '/Car/GetUserCars',
            type: "GET",
            cache: false,
            dataType: "json",
            success: function (data) {
                if (data.cars.length > 0)
                {
                    var lstCar = [];
                    for (var i = 0; i < data.cars.length; i++) {
                        car = new CarModel(data.cars[i]);
                        
                        lstCar.push(car);
                    }

                    self.cars(lstCar);
                }
            }
        });
       
    }

    self.getCar = function (carId)//get car from the list, when user click to an item
    {
        return $.map(self.cars(), function (val, i) {
            return val.id() == carId ? val: null;
        })[0];
    }

    self.showInsertModel = function ()
    {
        self.currentCar(new CarModel());//insert new car, then all should be empty
        self.isEdit(true);
    }

    self.hideValidationMessage = function ()
    {
        $('.validationMessage').hide();
    }

    self.showEditModel = function (car)
    {
        if (car.id() > 0) {
            self.currentCar(self.getCar(car.id()));//update currentCar on layout
            self.isEdit(true);
        }
    }

    self.updateCar = function () {

        if (self.currentCar().errors().length > 0) {
            self.currentCar().errors.showAllMessages();
            return false;
        }
        if (self.currentCar().id() > 0)
        {
            self.editCar(self.currentCar());//update on database
        }
        else
        {
            self.insertCar(self.currentCar());
        }

        $('#editCarModel').modal('hide');
    }

    self.insertCar = function ()
    {
        var formatPrice = self.currentCar().price();
        if (typeof (formatPrice) !== "undefined") {
            self.currentCar().price(formatPrice.replace(',', ''));
        }
        $.ajax({
            type: "POST",
            url: '/Car/InsertCar',
            data: JSON.stringify(ko.toJS(self.currentCar())),
            async: false,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                //refresh the list of car. todo: update only client side for layout
                self.getCars();
            },
            error: function () {
                //TODO: should implement an warning message
                console.log('An error was occurred on inserting');
            }
        });

        //refresh the list of car. todo: update only client side for layout
        self.getCars();
    }

    self.editCar = function()
    {
        var formatPrice = self.currentCar().price();
        if (typeof (formatPrice) !== "undefined") {
            self.currentCar().price(formatPrice.replace(',', ''));
        }
        
        $.ajax({
            type: "POST",
            url: '/Car/EditCar',
            data: JSON.stringify(ko.toJS(self.currentCar())),
            async: false,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                //refresh the list of car. todo: update only client side for layout
                self.getCars();
            },
            error: function () {
                //TODO: should implement an warning message
                console.log('An error was occurred on updating');
            }
        });
    }

    self.deleteCar = function (car)
    {
        $.ajax({
            type: "POST",
            url: '/Car/DeleteCar',
            data: JSON.stringify({ 'carId' : car.id()}),
            async: false,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                //refresh the list of car. todo: update only client side for layout
                self.getCars();
            },
            error: function () {
                //TODO: should implement an warning message
                console.log('An error was occurred on deleting');
            }
        });
    }

    self.errors = ko.validation.group(self, { deep: true }); //validate deep to the properties of current car on modal
    
}