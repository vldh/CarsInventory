$(document).ready(function () {
    $('.modal').on('show.bs.modal', function () {
        $(this).find('[autofocus]').focus();
    });
    $('.modal').on('hide.bs.modal', function () {
        
    });
})