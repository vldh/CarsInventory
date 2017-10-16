$(function () {

    $('#editCarModel').on('shown.bs.modal', function () {
        $(this).find('[autofocus]').focus();
        $('#datetimepickerYear').datetimepicker({
            viewMode: 'years',
            format: 'YYYY'
        });

    });
    $('.modal').on('hide.bs.modal', function () {

    });
    
    var carMangement = new CarManagement();
    ko.applyBindings(carMangement, document.getElementById('cars-management'));
    carMangement.getCars();
})