/**
 * @license Copyright (c) 2003-2022, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.removeButtons = "acceptchange,rejectchange";
    config.extraPlugins = 'filebrowser,popup,filetools,tableresize,pastefromgdocs,pastefromlibreoffice,pastefromword,tableselection,ajax,pastetools,textwatcher,xml';
    config.filebrowserUploadUrl = '/SOP/upload.aspx';
    config.filebrowserUploadMethod = 'form';
    config.extraPlugins = 'lite';
    //config.removePlugins = 'sourcearea,language,forms,div,iframe,about,preview,print,templates,exportpdf,save';
    config.removePlugins = 'sourcearea,language,forms,div,iframe,about,preview,print,templates,exportpdf,save,bidi,lang,showblocks,acceptchange,rejectchange';
    config.font_defaultLabel = 'Calibri';
    config.allowedContent = true;
    config.pasteFromWordRemoveStyles = false;
    config.pasteFromWordRemoveFontStyles = false;
    config.pasteFromWordRemoveStyles = false;
    config.disallowedContent = 'span[lang*]';

    config.toolbar = [
           //{ name: 'document', items: ['Preview', 'Print', '-'] },
           { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
           { name: 'colors', items: ['TextColor', 'BGColor'] },
           { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', '-', 'Undo', 'Redo'] },//'PasteText', 'PasteFromWord',
           { name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll', '-', 'Scayt'] },
           { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'CopyFormatting', 'RemoveFormat'] },
            '/',
           { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl', 'Language'] },
           { name: 'links', items: ['Link', 'Unlink'] },
           { name: 'insert', items: ['Image', 'Table', 'HorizontalRule'] },
           { name: 'tools', items: ['Maximize', 'ShowBlocks', 'PageBreak'] }
    ];

    var lite = config.lite = config.lite || {};
    lite.isTracking = true;
    lite.contextMenu = false;

    lite.tooltipTemplate = "%a by %u %t";
    try {
        lite.userName = document.getElementById("ctl00_ContentPlaceHolder1_hfCurrentUser").value;
        lite.userId = document.getElementById("ctl00_ContentPlaceHolder1_hfCurrentDate").value;
    } catch (e) {
        lite.userName = document.getElementById("hfCurrentUser").value;
        lite.userId = document.getElementById("hfCurrentDate").value;
    }
};
