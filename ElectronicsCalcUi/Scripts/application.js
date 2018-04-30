

$(document).ready(function () {

    var baseUrl = "http://localhost:57928/"
    var colors = [];
    var cboSignificantFigures;

    $.get(baseUrl + "api/ResistanceColorCodes", function (data) {
        colors = data;

        for (var i = 0; i < colors.length; i++) {
            colors[i].Id = data[i].Id;
            colors[i].RingColor = data[i].RingColor;
            colors[i].Code = data[i].Code;
            colors[i].SignificantFigure = data[i].SignificantFigure;
            colors[i].Multiplier = data[i].Multiplier;
            colors[i].Tolerance = data[i].Tolerance;
        }

    }).done(function () {
        cboCreate(colors);
    });

    //create the dropdown lists
    function cboCreate(colors) {
        //create cboboxes for sigfigs
        cboSignificantFigures(colors);

        //create cboboxes for multiplier and tolerance;
        cboMultiplier(colors);
        cboTolerance(colors);
        updateOhms();
    }

    //create a combobox template that will be used for sigfigs
    function cboSignificantFigures(colors) {
        var $div = $().add('<div>').addClass('band-combobox inline-block');

        //create a unique id for this div
        var id = s4();
        $div.attr('id', id);

        var $select = $().add('<select>').addClass('combobox form-control');

        for (var i = 0; i < colors.length; i++) {
            if (Number.isInteger(colors[i].SignificantFigure)) {
                var $option = $().add('<option>').val(colors[i].SignificantFigure);
                $option.text(colors[i].RingColor);
                $select.append($option);
                delete option;
            }
        }
        $div.append($select);
        cboSignificantFigures = $div.prop('outerHTML');  //keep template for later.

        //$div.find("option[value='0']").remove(); //the default and first combobox cannot have black (0) value.
        $('.band-combobox-sigfigs').append($div);


        var $band = $().add('<div>').attr('id', id).addClass('band');
        $band.css('background', 'linear-gradient(lightgrey, black, black, black, black)');
        $(".band-values").append($band);
    }

    //create a combobox template that will be used for multiplier
    function cboMultiplier(colors) {
        var $div = $().add('<div>').addClass('band-combobox inline-block full-width');

        //create a unique id for this div
        var id = s4();
        $div.attr('id', id);

        var $select = $().add('<select>').addClass('combobox form-control');

        //add a blank option
        var $option = $().add('<option>');
        $select.append($option);
        delete option;

        for (var i = 0; i < colors.length; i++) {
            if (colors[i].Multiplier) {
                var $option = $().add('<option>').val(colors[i].Multiplier);
                $option.text(colors[i].RingColor);
                $select.append($option);
                delete option;
            }
        }
        $div.append($select);
        $('.band-combobox-multiplier').append($div);

        var $band = $().add('<div>').attr('id', id).addClass('band hidden');
        $(".band-multiplier").append($band);
    }

    //create a combobox template that will be used for tolerance
    function cboTolerance(colors) {
        var $div = $().add('<div>').addClass('band-combobox inline-block full-width');

        //create a unique id for this div
        var id = s4();
        $div.attr('id', id);

        var $select = $().add('<select>').addClass('combobox form-control');

        //add a blank option
        var $option = $().add('<option>');
        $select.append($option);
        delete option;

        for (var i = 0; i < colors.length; i++) {
            if (colors[i].Tolerance) {
                var $option = $().add('<option>').val(colors[i].Tolerance);
                $option.text(colors[i].RingColor);
                $select.append($option);
                delete option;
            }
        }
        $div.append($select);
        $('.band-combobox-tolerance').append($div);

        var $band = $().add('<div>').attr('id', id).addClass('band hidden');
        $(".band-tolerance").append($band);
    }


    $('#add-band').on('click', function () {

        if ($('.band-combobox-sigfigs').find('.band-combobox').length < 3) {
            var id = s4();
            var template = cboSignificantFigures;
            var cbo = $(template).closest('div.band-combobox')
            $(cbo).attr('id', id);

            $(cbo).find("option[value='0']").remove();
            $(cbo).insertAfter($(".band-combobox-sigfigs").find('.band-desc'))

            var $band = $().add('<div>').attr('id', id).addClass('band');
            $band.css('background', 'linear-gradient(lightgrey, brown, brown, brown, black)');
            $('.band-values').prepend($band);

            //update fields.
            updateOhms();
        }
    });

    $('#rem-band').on('click', function () {
        //first find how many there are, don't remove the first one
        var cbos = $('.band-combobox-sigfigs').find('.band-combobox').length;

        if (cbos != 1) {
            //remove the first index
            $('.band-combobox-sigfigs').find('.band-combobox')[0].remove()
            $('.band-values').find('.band')[0].remove();

            //update fields
            updateOhms();
        }
    });


    $('.band-selection-container').on('change', 'select', function () {
        //get id of this select
        var id = $(this).parent().attr('id');

        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;  //not needed yet.
        var color = optionSelected.text();

        //set color of matching band from dropdown selection.
        var band = $('.band[id="' + id + '"]');

        if (color) {
            band.removeClass('hidden');
            band.css('background', 'linear-gradient(lightgrey, ' + color + ', ' + color + ', ' + color + ', black)');
        }
        else {
            band.addClass('hidden');
        }
        updateOhms();
    });

    //update fields with values.
    function updateOhms() {
        var ohmbands = {};
        ohmbands.bandColors = []

        //get colors of bands
        $('.band-selection-container').find('select').each(function (i) {

            //for each select element found, pull selected text as color and push into global color array
            var optionSelected = $("option:selected", this);
            var selected = optionSelected.text();

            //don't bring over any empty strings.
            if (selected.length > 0) {
                ohmbands.bandColors.push(selected);
            }
        });

        //there are no such thing as 2 band resistors.
        if (ohmbands.bandColors.length != 2) {
            $.post(baseUrl + "api/GetResistorValues/array", ohmbands).done(function (data) {
                obj = data;
                $("#resistance").html(convert(obj.ohms) + ' Ω');
                $("#tolerance").html('± ' + obj.tolerance + '%');
                $("#low").html(convert(obj.low) + ' Ω');
                $("#high").html(convert(obj.high) + ' Ω');
            });
        }
        else {
            $("#resistance").html('');
            $("#tolerance").html('');
            $("#low").html('');
            $("#high").html('');
        }
    }

    //this takes a number and parses for zeros to convert string to include metric symbol.
    function convert(number) {
        //find out how many zeros are are in the string
        var string = number.toString();
        if (number >= 1000) {  //only format if number is greater than a thousand.
            var hasZeros = string.match(/0/gi);  //can't do .length on null, so we'll check if zeros exist.
            if (hasZeros) {
                var zeros = string.match(/0/gi).length;
                var zerogroups = Math.floor(zeros / 3);

                if (zerogroups >= 1) {
                    var sizes = ['K', 'M'];
                    var i;
                    var j = 0;
                    for (i = 0; i < (zerogroups); i++) {
                        if (i < 2) {  //only for first two iterations.
                            j++;  //use second var so that we don't pass two, since zerogroups can be greater than two.
                            string = string.slice(0, -3);  //remove the trailing zeros
                        }
                    }
                    string = string + sizes[j - 1];  //depending on how many groups of zeros were removed, concat string with K or M.
                }
            }
        }
        return string;
    };

    //return 4 random characters, a unique id for elements.
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }

    //isInteger is not supported in some versions of IE added the polyfill
    Number.isInteger = Number.isInteger || function (value) {
        return typeof value === "number" &&
            isFinite(value) &&
            Math.floor(value) === value;
    };


    //.remove() quirky with IE versions tested, so added polyfill
    (function (arr) {
        arr.forEach(function (item) {
            if (item.hasOwnProperty('remove')) {
                return;
            }
            Object.defineProperty(item, 'remove', {
                configurable: true,
                enumerable: true,
                writable: true,
                value: function remove() {
                    this.parentNode.removeChild(this);
                }
            });
        });
    })([Element.prototype, CharacterData.prototype, DocumentType.prototype]);

});

