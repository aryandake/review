/**
 * @license Copyright (c) 2003-2022, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.extraPlugins = 'filebrowser,popup,filetools,tableresize,pastefromgdocs,pastefromlibreoffice,pastefromword,tableselection,ajax,pastetools,textwatcher,xml';
    config.filebrowserUploadMethod = 'form';
    config.filebrowserUploadUrl = '/SOP/upload.aspx';
    //config.extraPlugins = 'ajax,copyformatting,dialog,pastetools,textwatcher,xml';
    //config.extraPlugins = 'lite';
    //config.extraPlugins = 'pastefromword';

    config.removePlugins = 'sourcearea,language,forms,div,iframe,about,preview,print,templates,exportpdf,save,bidi,lang,showblocks';
    config.font_defaultLabel = 'Calibri';
    //config.image_previewText = '';
    //CKEDITOR.config.allowedContent = true
    config.allowedContent = true;
    config.pasteFromWordRemoveStyles = false;
    config.pasteFromWordRemoveFontStyles = false;
    config.pasteFromWordRemoveStyles = false;
    config.disallowedContent = 'span[lang*]';

    //CKEDITOR.config.fillEmptyBlocks = false;

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
           { name: 'insert', items: ['Table', 'HorizontalRule'] },
           { name: 'tools', items: ['Maximize', 'ShowBlocks','PageBreaks'] }
    ];
    //var lite = config.lite = config.lite || {};
    //lite.isTracking = false;
};
