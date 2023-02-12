/**
 * This modification of DataTables' standard two button pagination controls
 * adds a little animation effect to the paging action by redrawing the table
 * multiple times for each event, each draw progressing by one row until the
 * required point in the table is reached.
 *
 *  @name Scrolling navigation
 *  @summary Show page changes as a redraw of the table, scrolling records.
 *  @author [Allan Jardine](http://sprymedia.co.uk)
 *
 *  @example
 *    $(document).ready(function() {
 *        $('#example').dataTable( {
 *            "sPaginationType": "scrolling"
 *        } );
 *    } );
 */


/* Time between each scrolling frame */
$.fn.dataTableExt.oPagination.iTweenTime = 100;

$.fn.dataTableExt.oPagination.scrolling = {
	"fnInit": function ( oSettings, nPaging, fnCallbackDraw )
	{
		var oLang = oSettings.oLanguage.oPaginate;
		var oClasses = oSettings.oClasses;
		var fnClickHandler = function ( e ) {
			if ( oSettings.oApi._fnPageChange( oSettings, e.data.action ) )
			{
				fnCallbackDraw( oSettings );
			}
		};

		var sAppend = (!oSettings.bJUI) ?
			'<a class="'+oSettings.oClasses.sPagePrevDisabled+'" tabindex="'+oSettings.iTabIndex+'" role="button">'+oLang.sPrevious+'</a>'+
			'<a class="'+oSettings.oClasses.sPageNextDisabled+'" tabindex="'+oSettings.iTabIndex+'" role="button">'+oLang.sNext+'</a>'
			:
			'<a class="'+oSettings.oClasses.sPagePrevDisabled+'" tabindex="'+oSettings.iTabIndex+'" role="button"><span class="'+oSettings.oClasses.sPageJUIPrev+'"></span></a>'+
			'<a class="'+oSettings.oClasses.sPageNextDisabled+'" tabindex="'+oSettings.iTabIndex+'" role="button"><span class="'+oSettings.oClasses.sPageJUINext+'"></span></a>';
		$(nPaging).append( sAppend );

		var els = $('a', nPaging);
		var nPrevious = els[0],
			nNext = els[1];

		oSettings.oApi._fnBindAction( nPrevious, {action: "previous"}, function() {
			/* Disallow paging event during a current paging event */
			if ( typeof oSettings.iPagingLoopStart != 'undefined' && oSettings.iPagingLoopStart != -1 )
			{
				return;
			}

			oSettings.iPagingLoopStart = oSettings._iDisplayStart;
			oSettings.iPagingEnd = oSettings._iDisplayStart - oSettings._iDisplayLength;

			/* Correct for underrun */
			if ( oSettings.iPagingEnd < 0 )
			{
				oSettings.iPagingEnd = 0;
			}

			var iTween = $.fn.dataTableExt.oPagination.iTweenTime;
			var innerLoop = function () {
				if ( oSettings.iPagingLoopStart > oSettings.iPagingEnd ) {
					oSettings.iPagingLoopStart--;
					oSettings._iDisplayStart = oSettings.iPagingLoopStart;
					fnCallbackDraw( oSettings );
					setTimeout( function() { innerLoop(); }, iTween );
				} else {
					oSettings.iPagingLoopStart = -1;
				}
			};
			innerLoop();
		} );

		oSettings.oApi._fnBindAction( nNext, {action: "next"}, function() {
			/* Disallow paging event during a current paging event */
			if ( typeof oSettings.iPagingLoopStart != 'undefined' && oSettings.iPagingLoopStart != -1 )
			{
				return;
			}

			oSettings.iPagingLoopStart = oSettings._iDisplayStart;

			/* Make sure we are not over running the display array */
			if ( oSettings._iDisplayStart + oSettings._iDisplayLength < oSettings.fnRecordsDisplay() )
			{
				oSettings.iPagingEnd = oSettings._iDisplayStart + oSettings._iDisplayLength;
			}

			var iTween = $.fn.dataTableExt.oPagination.iTweenTime;
			var innerLoop = function () {
				if ( oSettings.iPagingLoopStart < oSettings.iPagingEnd ) {
					oSettings.iPagingLoopStart++;
					oSettings._iDisplayStart = oSettings.iPagingLoopStart;
					fnCallbackDraw( oSettings );
					setTimeout( function() { innerLoop(); }, iTween );
				} else {
					oSettings.iPagingLoopStart = -1;
				}
			};
			innerLoop();
		} );
	},

	"fnUpdate": function ( oSettings, fnCallbackDraw )
	{
		if ( !oSettings.aanFeatures.p )
		{
			return;
		}

		/* Loop over each instance of the pager */
		var an = oSettings.aanFeatures.p;
		for ( var i=0, iLen=an.length ; i<iLen ; i++ )
		{
			if ( an[i].childNodes.length !== 0 )
			{
				an[i].childNodes[0].className =
					( oSettings._iDisplayStart === 0 ) ?
					oSettings.oClasses.sPagePrevDisabled : oSettings.oClasses.sPagePrevEnabled;

				an[i].childNodes[1].className =
					( oSettings.fnDisplayEnd() == oSettings.fnRecordsDisplay() ) ?
					oSettings.oClasses.sPageNextDisabled : oSettings.oClasses.sPageNextEnabled;
			}
		}
	}
};


//User Input
$.fn.dataTableExt.oPagination.input = {
    "fnInit": function (oSettings, nPaging, fnCallbackDraw) {
        var nFirst = document.createElement('span');
        var nPrevious = document.createElement('span');
        var nNext = document.createElement('span');
        var nLast = document.createElement('span');
        var nInput = document.createElement('input');
        var nPage = document.createElement('span');
        var nOf = document.createElement('span');

        nFirst.innerHTML = oSettings.oLanguage.oPaginate.sFirst;
        nPrevious.innerHTML = oSettings.oLanguage.oPaginate.sPrevious;
        nNext.innerHTML = oSettings.oLanguage.oPaginate.sNext;
        nLast.innerHTML = oSettings.oLanguage.oPaginate.sLast;

        nFirst.className = "paginate_button first";
        nPrevious.className = "paginate_button previous";
        nNext.className = "paginate_button next";
        nLast.className = "paginate_button last";
        nOf.className = "paginate_of";
        nPage.className = "paginate_page";

        if (oSettings.sTableId !== '') {
            nPaging.setAttribute('id', oSettings.sTableId + '_paginate');
            nPrevious.setAttribute('id', oSettings.sTableId + '_previous');
            nPrevious.setAttribute('id', oSettings.sTableId + '_previous');
            nNext.setAttribute('id', oSettings.sTableId + '_next');
            nLast.setAttribute('id', oSettings.sTableId + '_last');
        }

        nInput.type = "text";
        nInput.style.width = "15px";
        nInput.style.display = "inline";
        nPage.innerHTML = "Page ";

        nPaging.appendChild(nFirst);
        nPaging.appendChild(nPrevious);
        nPaging.appendChild(nPage);
        nPaging.appendChild(nInput);
        nPaging.appendChild(nOf);
        nPaging.appendChild(nNext);
        nPaging.appendChild(nLast);

        $(nFirst).click(function () {
            oSettings.oApi._fnPageChange(oSettings, "first");
            fnCallbackDraw(oSettings);
        });

        $(nPrevious).click(function () {
            oSettings.oApi._fnPageChange(oSettings, "previous");
            fnCallbackDraw(oSettings);
        });

        $(nNext).click(function () {
            oSettings.oApi._fnPageChange(oSettings, "next");
            fnCallbackDraw(oSettings);
        });

        $(nLast).click(function () {
            oSettings.oApi._fnPageChange(oSettings, "last");
            fnCallbackDraw(oSettings);
        });

        $(nInput).keyup(function (e) {

            if (e.which == 38 || e.which == 39) {
                this.value++;
            }
            else if ((e.which == 37 || e.which == 40) && this.value > 1) {
                this.value--;
            }

            if (this.value == "" || this.value.match(/[^0-9]/)) {
                /* Nothing entered or non-numeric character */
                return;
            }

            var iNewStart = oSettings._iDisplayLength * (this.value - 1);
            if (iNewStart > oSettings.fnRecordsDisplay()) {
                /* Display overrun */
                oSettings._iDisplayStart = (Math.ceil((oSettings.fnRecordsDisplay() - 1) /
                    oSettings._iDisplayLength) - 1) * oSettings._iDisplayLength;
                fnCallbackDraw(oSettings);
                return;
            }

            oSettings._iDisplayStart = iNewStart;
            fnCallbackDraw(oSettings);
        });

        /* Take the brutal approach to cancelling text selection */
        $('span', nPaging).bind('mousedown', function () { return false; });
        $('span', nPaging).bind('selectstart', function () { return false; });
    },


    "fnUpdate": function (oSettings, fnCallbackDraw) {
        if (!oSettings.aanFeatures.p) {
            return;
        }
        var iPages = Math.ceil((oSettings.fnRecordsDisplay()) / oSettings._iDisplayLength);
        var iCurrentPage = Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength) + 1;

        /* Loop over each instance of the pager */
        var an = oSettings.aanFeatures.p;
        for (var i = 0, iLen = an.length ; i < iLen ; i++) {
            var spans = an[i].getElementsByTagName('span');
            var inputs = an[i].getElementsByTagName('input');
            spans[3].innerHTML = " of " + iPages
            inputs[0].value = iCurrentPage;
        }
    }
};


//list box

$.fn.dataTableExt.oPagination.listbox = {
    /*
     * Function: oPagination.listbox.fnInit
     * Purpose:  Initalise dom elements required for pagination with listbox input
     * Returns:  -
     * Inputs:   object:oSettings - dataTables settings object
     *             node:nPaging - the DIV which contains this pagination control
     *             function:fnCallbackDraw - draw function which must be called on update
     */
    "fnInit": function (oSettings, nPaging, fnCallbackDraw) {
        var nInput = document.createElement('select');
        var nPage = document.createElement('span');
        var nOf = document.createElement('span');


        var nPrevious = document.createElement('span');
        var nNext = document.createElement('span');
        
        nPrevious.innerHTML = "<i class='fa fa-chevron-left leftarrow'></i>"; // "<< " + oSettings.oLanguage.oPaginate.sPrevious;
        nNext.innerHTML = "<i class='fa fa-chevron-right rightarrow'></i>"; //oSettings.oLanguage.oPaginate.sNext + " >>" ;


        nPrevious.className = "paginate_button previous";
        nNext.className = "paginate_button next";



        nOf.className = "paginate_of";
        nPage.className = "paginate_page";
        if (oSettings.sTableId !== '') {
            nPaging.setAttribute('id', oSettings.sTableId + '_paginate');
        }
        nInput.style.display = "inline";
        
        
        nPaging.appendChild(nPrevious);
        nPage.innerHTML = "  ";
        nPaging.appendChild(nPage);
        nPaging.appendChild(nInput);
        nPaging.appendChild(nOf);
        nPaging.appendChild(nNext);


        $(nPrevious).click(function () {
            if (this.className == "paginate_disabled_previous") {
                return false;
            }
            oSettings.oApi._fnPageChange(oSettings, "previous");
            fnCallbackDraw(oSettings);
        });

        $(nNext).click(function () {
            if (this.className == "paginate_disabled_next") {
                return false;
            }
            oSettings.oApi._fnPageChange(oSettings, "next");
            fnCallbackDraw(oSettings);
        })

        $(nInput).change(function (e) { // Set DataTables page property and redraw the grid on listbox change event.
            window.scroll(0, 0); //scroll to top of page
            if (this.value === "" || this.value.match(/[^0-9]/)) { /* Nothing entered or non-numeric character */
                return;
            }
            var iNewStart = oSettings._iDisplayLength * (this.value - 1);
            if (iNewStart > oSettings.fnRecordsDisplay()) { /* Display overrun */
                oSettings._iDisplayStart = (Math.ceil((oSettings.fnRecordsDisplay() - 1) / oSettings._iDisplayLength) - 1) * oSettings._iDisplayLength;
                fnCallbackDraw(oSettings);
                return;
            }
            oSettings._iDisplayStart = iNewStart;
            fnCallbackDraw(oSettings);
        }); /* Take the brutal approach to cancelling text selection */
        $('span', nPaging).bind('mousedown', function () {
            return false;
        });
        $('span', nPaging).bind('selectstart', function () {
            return false;
        });
    },

    /*
     * Function: oPagination.listbox.fnUpdate
     * Purpose:  Update the listbox element
     * Returns:  -
     * Inputs:   object:oSettings - dataTables settings object
     *             function:fnCallbackDraw - draw function which must be called on update
     */
    "fnUpdate": function (oSettings, fnCallbackDraw) {
        if (!oSettings.aanFeatures.p) {
            return;
        }
        var iPages = Math.ceil((oSettings.fnRecordsDisplay()) / oSettings._iDisplayLength);
        var iCurrentPage = Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength) + 1; /* Loop over each instance of the pager */
        
        var an = oSettings.aanFeatures.p;
        
        for (var i = 0, iLen = an.length; i < iLen; i++) {
            var spans = an[i].getElementsByTagName('span');
            var inputs = an[i].getElementsByTagName('select');
            var elSel = inputs[0];
            if (elSel.options.length != iPages) {
                elSel.options.length = 0; //clear the listbox contents
                for (var j = 0; j < iPages; j++) { //add the pages
                    var oOption = document.createElement('option');
                    oOption.text = j + 1;
                    oOption.value = j + 1;
                    try {
                        elSel.add(oOption, null); // standards compliant; doesn't work in IE
                    } catch (ex) {
                        elSel.add(oOption); // IE only
                    }
                }
                spans[2].innerHTML = " ";//"  of " + iPages;
            }
            elSel.value = iCurrentPage;

            //0 is previous span
            if (oSettings._iDisplayStart === 0) {
                spans[0].className = "paginate_disabled_previous";
            }
            else {
                spans[0].className = "paginate_enabled_previous ";
            }

            //3 is next span
            if (oSettings.fnDisplayEnd() == oSettings.fnRecordsDisplay()) {
                spans[3].className = "paginate_disabled_next";
            }
            else {
                
                spans[3].className = "paginate_enabled_next";
            }
            
        }
    }
};


//four buttons
$.fn.dataTableExt.oPagination.four_button = {
    "fnInit": function (oSettings, nPaging, fnCallbackDraw) {
        var nFirst = document.createElement('span');
        var nPrevious = document.createElement('span');
        var nNext = document.createElement('span');
        var nLast = document.createElement('span');

        nFirst.appendChild(document.createTextNode(oSettings.oLanguage.oPaginate.sFirst));
        nPrevious.appendChild(document.createTextNode(oSettings.oLanguage.oPaginate.sPrevious));
        nNext.appendChild(document.createTextNode(oSettings.oLanguage.oPaginate.sNext));
        nLast.appendChild(document.createTextNode(oSettings.oLanguage.oPaginate.sLast));

        nFirst.className = "paginate_button first";
        nPrevious.className = "paginate_button previous";
        nNext.className = "paginate_button next";
        nLast.className = "paginate_button last";

        nPaging.appendChild(nFirst);
        nPaging.appendChild(nPrevious);
        nPaging.appendChild(nNext);
        nPaging.appendChild(nLast);

        $(nFirst).click(function () {
            oSettings.oApi._fnPageChange(oSettings, "first");
            fnCallbackDraw(oSettings);
        });

        $(nPrevious).click(function () {
            oSettings.oApi._fnPageChange(oSettings, "previous");
            fnCallbackDraw(oSettings);
        });

        $(nNext).click(function () {
            oSettings.oApi._fnPageChange(oSettings, "next");
            fnCallbackDraw(oSettings);
        });

        $(nLast).click(function () {
            oSettings.oApi._fnPageChange(oSettings, "last");
            fnCallbackDraw(oSettings);
        });

        /* Disallow text selection */
        $(nFirst).bind('selectstart', function () { return false; });
        $(nPrevious).bind('selectstart', function () { return false; });
        $(nNext).bind('selectstart', function () { return false; });
        $(nLast).bind('selectstart', function () { return false; });
    },


    "fnUpdate": function (oSettings, fnCallbackDraw) {
        if (!oSettings.aanFeatures.p) {
            return;
        }

        /* Loop over each instance of the pager */
        var an = oSettings.aanFeatures.p;
        for (var i = 0, iLen = an.length ; i < iLen ; i++) {
            var buttons = an[i].getElementsByTagName('span');
            if (oSettings._iDisplayStart === 0) {
                buttons[0].className = "paginate_disabled_previous";
                buttons[1].className = "paginate_disabled_previous";
            }
            else {
                buttons[0].className = "paginate_enabled_previous";
                buttons[1].className = "paginate_enabled_previous";
            }

            if (oSettings.fnDisplayEnd() == oSettings.fnRecordsDisplay()) {
                buttons[2].className = "paginate_disabled_next";
                buttons[3].className = "paginate_disabled_next";
            }
            else {
                buttons[2].className = "paginate_enabled_next";
                buttons[3].className = "paginate_enabled_next";
            }
        }
    }
};


//eclipilse

$.extend($.fn.dataTableExt.oStdClasses, {
    'sPageEllipsis': 'paginate_ellipsis',
    'sPageNumber': 'paginate_number',
    'sPageNumbers': 'paginate_numbers'
});

$.fn.dataTableExt.oPagination.ellipses = {
    'oDefaults': {
        'iShowPages': 5
    },
    'fnClickHandler': function (e) {
        var fnCallbackDraw = e.data.fnCallbackDraw,
            oSettings = e.data.oSettings,
            sPage = e.data.sPage;

        if ($(this).is('[disabled]')) {
            return false;
        }

        oSettings.oApi._fnPageChange(oSettings, sPage);
        fnCallbackDraw(oSettings);

        return true;
    },
    // fnInit is called once for each instance of pager
    'fnInit': function (oSettings, nPager, fnCallbackDraw) {
        var oClasses = oSettings.oClasses,
            oLang = oSettings.oLanguage.oPaginate,
            that = this;

        var iShowPages = oSettings.oInit.iShowPages || this.oDefaults.iShowPages,
            iShowPagesHalf = Math.floor(iShowPages / 2);

        $.extend(oSettings, {
            _iShowPages: iShowPages,
            _iShowPagesHalf: iShowPagesHalf,
        });

        var oFirst = $('<a class="' + oClasses.sPageButton + ' ' + oClasses.sPageFirst + '">' + oLang.sFirst + '</a>'),
            oPrevious = $('<a class="' + oClasses.sPageButton + ' ' + oClasses.sPagePrevious + '">' + oLang.sPrevious + '</a>'),
            oNumbers = $('<span class="' + oClasses.sPageNumbers + '"></span>'),
            oNext = $('<a class="' + oClasses.sPageButton + ' ' + oClasses.sPageNext + '">' + oLang.sNext + '</a>'),
            oLast = $('<a class="' + oClasses.sPageButton + ' ' + oClasses.sPageLast + '">' + oLang.sLast + '</a>');

        oFirst.click({ 'fnCallbackDraw': fnCallbackDraw, 'oSettings': oSettings, 'sPage': 'first' }, that.fnClickHandler);
        oPrevious.click({ 'fnCallbackDraw': fnCallbackDraw, 'oSettings': oSettings, 'sPage': 'previous' }, that.fnClickHandler);
        oNext.click({ 'fnCallbackDraw': fnCallbackDraw, 'oSettings': oSettings, 'sPage': 'next' }, that.fnClickHandler);
        oLast.click({ 'fnCallbackDraw': fnCallbackDraw, 'oSettings': oSettings, 'sPage': 'last' }, that.fnClickHandler);

        // Draw
        $(nPager).append(oFirst, oPrevious, oNumbers, oNext, oLast);
    },
    // fnUpdate is only called once while table is rendered
    'fnUpdate': function (oSettings, fnCallbackDraw) {
        var oClasses = oSettings.oClasses,
            that = this;

        var tableWrapper = oSettings.nTableWrapper;

        // Update stateful properties
        this.fnUpdateState(oSettings);

        if (oSettings._iCurrentPage === 1) {
            $('.' + oClasses.sPageFirst, tableWrapper).attr('disabled', true);
            $('.' + oClasses.sPagePrevious, tableWrapper).attr('disabled', true);
        } else {
            $('.' + oClasses.sPageFirst, tableWrapper).removeAttr('disabled');
            $('.' + oClasses.sPagePrevious, tableWrapper).removeAttr('disabled');
        }

        if (oSettings._iTotalPages === 0 || oSettings._iCurrentPage === oSettings._iTotalPages) {
            $('.' + oClasses.sPageNext, tableWrapper).attr('disabled', true);
            $('.' + oClasses.sPageLast, tableWrapper).attr('disabled', true);
        } else {
            $('.' + oClasses.sPageNext, tableWrapper).removeAttr('disabled');
            $('.' + oClasses.sPageLast, tableWrapper).removeAttr('disabled');
        }

        var i, oNumber, oNumbers = $('.' + oClasses.sPageNumbers, tableWrapper);

        // Erase
        oNumbers.html('');

        for (i = oSettings._iFirstPage; i <= oSettings._iLastPage; i++) {
            oNumber = $('<a class="' + oClasses.sPageButton + ' ' + oClasses.sPageNumber + '">' + oSettings.fnFormatNumber(i) + '</a>');

            if (oSettings._iCurrentPage === i) {
                oNumber.attr('active', true).attr('disabled', true);
            } else {
                oNumber.click({ 'fnCallbackDraw': fnCallbackDraw, 'oSettings': oSettings, 'sPage': i - 1 }, that.fnClickHandler);
            }

            // Draw
            oNumbers.append(oNumber);
        }

        // Add ellipses
        if (1 < oSettings._iFirstPage) {
            oNumbers.prepend('<span class="' + oClasses.sPageEllipsis + '">...</span>');
        }

        if (oSettings._iLastPage < oSettings._iTotalPages) {
            oNumbers.append('<span class="' + oClasses.sPageEllipsis + '">...</span>');
        }
    },
    // fnUpdateState used to be part of fnUpdate
    // The reason for moving is so we can access current state info before fnUpdate is called
    'fnUpdateState': function (oSettings) {
        var iCurrentPage = Math.ceil((oSettings._iDisplayStart + 1) / oSettings._iDisplayLength),
            iTotalPages = Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength),
            iFirstPage = iCurrentPage - oSettings._iShowPagesHalf,
            iLastPage = iCurrentPage + oSettings._iShowPagesHalf;

        if (iTotalPages < oSettings._iShowPages) {
            iFirstPage = 1;
            iLastPage = iTotalPages;
        } else if (iFirstPage < 1) {
            iFirstPage = 1;
            iLastPage = oSettings._iShowPages;
        } else if (iLastPage > iTotalPages) {
            iFirstPage = (iTotalPages - oSettings._iShowPages) + 1;
            iLastPage = iTotalPages;
        }

        $.extend(oSettings, {
            _iCurrentPage: iCurrentPage,
            _iTotalPages: iTotalPages,
            _iFirstPage: iFirstPage,
            _iLastPage: iLastPage
        });
    }
};