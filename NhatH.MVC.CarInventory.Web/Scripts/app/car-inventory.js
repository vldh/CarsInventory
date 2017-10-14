var CarModel = function (car) {
    var self = this;
    self.id = ko.observable();
    self.brand = ko.observable();
    self.model = ko.observable();
    self.price = ko.observable();
    self.visible = ko.observable(true);
    self.init = function ()
    {
        if (car != null)
        {
            self.id(car.ID);
            self.brand(car.Brand); //todo: make json controller return Camel case
            self.model(car.Model);
            self.price(car.Price);
        }
    }

    self.init();
}

var CarManagement = function ()
{
    var self = this;
    self.cars = ko.observableArray([]);
    self.isEdit = ko.observable(false);
    self.currentCar = ko.observable(new CarModel());
    self.searchText = ko.observable();
    self.searchText.subscribe(function (newText) {
        
        $.each(self.cars(), function (i, val) {
            val.brand().indexOf(newText) != -1 || val.model().indexOf(newText) != -1 ? val.visible(true) : val.visible(false);
        });
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

    self.showEditModel = function (car)
    {
        if (car.id() > 0) {
            self.currentCar(self.getCar(car.id()));//update currentCar on layout
            self.isEdit(true);
        }
    }

    self.updateCar = function () {

        if (self.currentCar().id() > 0)
        {
            self.editCar(self.currentCar());//update on database
        }
        else
        {
            self.insertCar(self.currentCar());
        }
    }

    self.insertCar = function ()
    {
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
              
            }
        });

    }

    self.editCar = function()
    {
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

            }
        });
    }

}