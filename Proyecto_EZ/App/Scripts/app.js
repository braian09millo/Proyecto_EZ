/// <reference path="jquery-3.3.1.min.js" />
$(document).ready(function () {

    var flag = true;
    var index = 1;

    $(window).on('resize', function () {
        if ($(this).width() > 768) {
            $('.menu-bar').css('display', 'flex');
        } else {
            $('.menu-bar').css('display', 'none')
        }
    });

    $('#btn').click(function () {
        $('.menu-bar').slideToggle(700);
    });


    $('.dropdown-toggle').click(function () {

        console.log($(this).attr('aria-expanded'));
        console.log(this);

        if ($(this).attr('aria-expanded') === "false") {
            var menuID = $(this).attr('id'),
            parentScope = $(this).parent().parent();
            parentScope.find('ul').removeClass('open').attr('aria-expanded', false);
            parentScope.find('ul[aria-labelledby=' + menuID + ']').addClass('open')
            parentScope.find('button[aria-expanded=true]').attr('aria-expanded', false);
            $(this).attr('aria-expanded', true);
        }
        else {
            $(this).attr('aria-expanded', false);
            var menuID = $(this).attr('id');
            parentScope = $(this).parent().parent();

            $(this).siblings().each(function () {
                console.log($(this).siblings());
                if ($(this).attr('aria-labelledby') == menuID) {
                    $(this).removeClass('open');
                }
            });
        }
    });

    var showDivs = function (value) {

        var parent = $('.item').parent();
        var sliderId = parent.attr('id');
        console.log(sliderId);

        if (value > $('#' + sliderId + '> .item').length) { index = 1; }
        if (value < 1) { index = $('#' + sliderId + '> .item').length; }

        $('#' + sliderId + '> .item').each(function () {
            $(this).css('display', 'none');
        });

        $($('#' + sliderId + '> .item')[index - 1]).css('display', 'block');
    }

    showDivs(index);

    $('.btn-carousel-right').click(function () {
        showDivs(index += 1);
    });

    $('.btn-carousel-left').click(function () {
        showDivs(index += -1);
    });

});