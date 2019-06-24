/// <reference path="jquery-3.3.1.js" />

function isNumber(value)
{
    var intRegex = /[0-9 -()+]+$/;
    return intRegex.test(value);
}

function isName(value)
{
    var nameRegex = /^([A-Z][A-Za-z.'\- ]+) (?:([A-Z][A-Za-z.'\-]+) )?([A-Z][A-Za-z.'\-]+)$/;
    return nameRegex.test(value);
}

function isEmail(value)
{
    var mailRegex = /^[\w%_\-.\d]+@[\w.\-]+.[A-Za-z]{2,6}$/;
    return mailRegex = 
}