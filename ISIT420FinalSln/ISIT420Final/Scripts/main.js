$(document).ready(function () {
    getAllYears();
});

const noDataReturned = "no data";
const highChanceReturned = "high";
const pocDataType = "POC"; // Perceptions of Corruption
const hlDataType = "HL" // Healthy Life
const peDataType = "PE" // Positive Effect

function getAllYears() {
    $.getJSON("api/worldhealth/GetAllYears")
        .done(function (data) {
            // On success, 'data' contains a list of all years for U.S. only
            $.each(data, function (index, value) {
                $('#selectYear').append(`<option>${value}</option>`);
            });

            getAllSongTitlesForYear();
        });
}

$(document).on('change', '#selectYear', function () {
    // clear songTitle options when different year is selected
    $('#selectSong').children('option').remove();

    getAllSongTitlesForYear();
});

function getAllSongTitlesForYear() {
    var year = parseInt($('#selectYear option:selected').text());
    $.getJSON("api/songs/GetAllSongTitlesForYear?year=" + year)
        .done(function (data) {
            // On success, 'data' contains a list of all Song Titles
            $.each(data, function (index, value) {
                $('#selectSong').append(`<option>${value}</option>`);
            });
        });
}

function getPerceptionsOfCorruption() {
    var year = parseInt($('#selectYear option:selected').text());
    var song = $('#selectSong option:selected').text();
    var dataType = pocDataType;
    $.getJSON("api/perceptionsofcorruption/GetPerceptionsOfCorruption?year=" + year + "&song=" + song)
        .done(function (data) {
            if (data.Status === noDataReturned) {
                setNoDataMessage(dataType);
            }
            else if (data.Status === highChanceReturned) {
                setHighMessage(dataType, data.Year, data.PocValue, data.SongTitle);
            }
            // slim Chance Returned
            else {
                setSlimMessage(dataType, data.Year, data.PocValue, data.SongTitle);
            }
        });
}

function getHealthyLife() {
    var year = parseInt($('#selectYear option:selected').text());
    var song = $('#selectSong option:selected').text();
    var dataType = hlDataType;
    $.getJSON("api/healthylife/GetHealthyLife?year=" + year + "&song=" + song)
        .done(function (data) {
            if (data.Status === noDataReturned) {
                setNoDataMessage(dataType);
            }
            else if (data.Status === highChanceReturned) {
                setHighMessage(dataType, data.Year, data.HlValue, data.SongTitle);
            }
            // slim Chance Returned
            else {
                setSlimMessage(dataType, data.Year, data.HlValue, data.SongTitle);
            }
        });
}

function getPositiveEffect() {
    var year = parseInt($('#selectYear option:selected').text());
    var song = $('#selectSong option:selected').text();
    var dataType = peDataType;
    $.getJSON("api/positiveeffect/GetPositiveEffect?year=" + year + "&song=" + song)
        .done(function (data) {
            if (data.Status === noDataReturned) {
                setNoDataMessage(dataType);
            }
            else if (data.Status === highChanceReturned) {
                setHighMessage(dataType, data.Year, data.PeValue, data.SongTitle);
            }
            // slim Chance Returned
            else {
                setSlimMessage(dataType, data.Year, data.PeValue, data.SongTitle);
            }
        });
}

function setNoDataMessage(dataType) {
    var mssg = "No data has been recorded for this query. Try a different year and/or song.";

    if (dataType === pocDataType) {
        $('#pocAnalysisResult').css({ "color": "red", "font-size": "13pt" });
        $('#pocAnalysisResult').text(mssg);
    }
    else if (dataType === hlDataType) {
        $('#healthyLifeAnalysisResult').css({ "color": "red", "font-size": "13pt" });
        $('#healthyLifeAnalysisResult').text(mssg);
    }
    else {
        $('#positiveEffectAnalysisResult').css({ "color": "red", "font-size": "13pt" });
        $('#positiveEffectAnalysisResult').text(mssg);
    }
}

function setHighMessage(dataType, year, whValue, songTitle) {
    var mssg = "Chances are high!";

    if (dataType === pocDataType) {
        $('#pocAnalysisResult').text(mssg).append(`<br /><br />Perceptions of Corruption for the year <b>${year}</b> is:<b>
            ${whValue}</b><br />Song <b>${songTitle}</b> <i>DOES</i> contain explicit lyrics`).css({ "color": "black", "font-size": "13pt" });
    }
    else if (dataType === hlDataType) {
        $('#healthyLifeAnalysisResult').text(mssg).append(`<br /><br />Healthy life expectancy at birth for the year <b>${year}</b> is:<b>
            ${whValue}</b><br />Song <b>${songTitle}</b> has <i>HIGH</i> energy`).css({ "color": "black", "font-size": "13pt" });
    }
    else {
        $('#positiveEffectAnalysisResult').text(mssg).append(`<br /><br />Positive Effect for the year <b>${year}</b> is:<b>
            ${whValue}</b><br />Song <b>${songTitle}</b> has <i>HIGH</i> danceability`).css({ "color": "black", "font-size": "13pt" });
    }
}

function setSlimMessage(dataType, year, whValue, songTitle) {
    var mssg = "Chances are slim!";
    if (dataType === pocDataType) {
        $('#pocAnalysisResult').text(mssg).append(`<br /><br />Perceptions of Corruption for the year <b>${year}</b> is:<b>
            ${whValue}</b><br />Song <b>${songTitle}</b> <i>DOES NOT</i> contain explicit lyrics`).css({ "color": "black", "font-size": "13pt" });
    }
    else if (dataType === hlDataType) {
        $('#healthyLifeAnalysisResult').text(mssg).append(`<br /><br />Healthy life expectancy at birth for the year <b>${year}</b> is:<b>
            ${whValue}</b><br />Song <b>${songTitle}</b> has <i>LOW</i> energy`).css({ "color": "black", "font-size": "13pt" });
    }
    else {
        $('#positiveEffectAnalysisResult').text(mssg).append(`<br /><br />Positive Effect for the year <b>${year}</b> is:<b>
            ${whValue}</b><br />Song <b>${songTitle}</b> has <i>LOW</i> danceability`).css({ "color": "black", "font-size": "13pt" });
    }
}
