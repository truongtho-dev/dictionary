function LanguageCollection(dto) {
    var self = this;

    self.languages = ko.observableArray(dto.languages.map(x => new Language(x)));
}

function Language(dto) {
    var self = this;

    self.id = ko.observable(dto.id);
    self.code = ko.observable(dto.code);
    self.name = ko.observable(dto.name);
    self.sourceDictionaries = ko.observableArray(dto.sourceDictionaries.map(x => new Dictionary(x)));
}

function Dictionary(dto) {
    var self = this;

    self.id = dto.id;
    self.name = dto.name;
    self.description = dto.description;
    self.selected = ko.observable(true);

    self.class = ko.pureComputed(function() {
        return self.selected() ? 'btn-primary' : 'btn-outline-primary';
    });

    self.toggleSelection = function() {
        self.selected(!self.selected());
    }
}

function DictionaryQueryResult(dto) {
    var self = this;

    self.dictionaryId = ko.observable(dto.dictionaryId);
    self.dictionaryName = ko.observable(dto.dictionaryName);
    self.results = ko.observable(dto.results.map(x => new DictionaryEntry(x)));
}

function DictionaryEntry(dto) {
    var self = this;

    self.id = dto.id;
    self.word = dto.word;
    self.meaning = dto.meaning;
}

function TranslateResult(dto) {
    var self = this;

    self.keyword = ko.observable(dto.keyword);
    self.results = ko.observableArray(dto.results.map(x => new DictionaryQueryResult(x)));
}

function HomeViewModel(app, dataModel) {
    var self = this;

    self.dictionaries = ko.observable();
    self.languages = ko.observableArray();

    self.languageCollection = ko.observable();

    self.chosenLanguage = ko.observable();

    self.keyword = ko.observable().extend({
        minLength: 1,
        maxLength: 100,
        required: true
    });

    self.translateResult = ko.observable();

    self.selectLanguage = function(language) {
        self.chosenLanguage(language);
    };

    self.query = function() {
        if (self.keyword.isValid()) {
            var selectedDictionaryIds = self.chosenLanguage().sourceDictionaries().filter(d => d.selected() === true).map(d => d.id);

            $.post({
                url: app.dataModel.queryTranslate,
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                },
                data: { keyword: self.keyword(), dictionaryIds: selectedDictionaryIds},
                success: function (data) {
                    self.translateResult(new TranslateResult(data));
                }
            });
        }
    };

    Sammy(function () {
        this.get('/', function() { // initial state
            $.get({
                url: app.dataModel.queryLanguages,
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                },
                success: function (data) {
                    self.languageCollection(new LanguageCollection(data));
                    self.chosenLanguage(self.languageCollection().languages()[0]);
                }
            });
        });
    });

    return self;
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});
