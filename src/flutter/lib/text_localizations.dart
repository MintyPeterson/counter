import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';

/// Text localisations for multi-language support.
class TextLocalizations {

  final Locale locale;
  TextLocalizations(this.locale);

  static TextLocalizations of(BuildContext context) {
    return Localizations.of<TextLocalizations>(context, TextLocalizations)!;
  }

  static const _localizedValues = <String, Map<String, String>>{
    'en': {
      'addEntry': 'Add entry',
      'addEntryFailed': 'Your entry could not be added. Please try again.',
      'copyEntry': 'Copy entry',
      'copyingEntry': 'Copying entry...',
      'counter': 'Counter',
      'date': 'Date',
      'deleteEntryFailed': 'Your entry could not be deleted. Please try again.',
      'entry': 'Entry',
      'isEstimate': 'Estimate',
      'loadingEntry': 'Loading entry...',
      'loadingEntries': 'Loading your entries...',
      'mustBeWholeNumber': 'Must be a whole number',
      'noEntriesToDisplay': 'There are no entries to display.',
      'notes': 'Notes',
      'ok': 'OK',
      'refresh': 'Refresh',
      'retry': 'Retry',
      'retryCopyEntry': 'Your entry could not be copied. Please try again.',
      'retryLoadEntries': 'Your entries could not be loaded. Please try again.',
      'retryLoadEntry': 'Your entry could not be loaded. Please try again.',
      'required': 'Required',
      'search': 'Search...',
      'sessionExpired': 'Your session has expired. Please sign in.',
      'signIn': 'Sign in',
      'signInTitle': 'Counter',
      'signInInstructions': 'Sign in to get started.',
      'signOut': 'Sign out',
      'untitledEntry': 'Untitled',
      'updateEntry': 'Update entry',
      'updateEntryFailed': 'Your entry could not be updated. Please try again.',
      'welcome': 'Welcome',
    },
  };

  static List<String> languages() => _localizedValues.keys.toList();

  String get addEntry {
    return _localizedValues[locale.languageCode]!['addEntry']!;
  }

  String get addEntryFailed {
    return _localizedValues[locale.languageCode]!['addEntryFailed']!;
  }

  String get copyEntry {
    return _localizedValues[locale.languageCode]!['copyEntry']!;
  }

  String get copyingEntry {
    return _localizedValues[locale.languageCode]!['copyingEntry']!;
  }

  String get counter {
    return _localizedValues[locale.languageCode]!['counter']!;
  }

  String get date {
    return _localizedValues[locale.languageCode]!['date']!;
  }

  String get deleteEntryFailed {
    return _localizedValues[locale.languageCode]!['deleteEntryFailed']!;
  }

  String get entry {
    return _localizedValues[locale.languageCode]!['entry']!;
  }

  String get isEstimate {
    return _localizedValues[locale.languageCode]!['isEstimate']!;
  }

  String get loadingEntry {
    return _localizedValues[locale.languageCode]!['loadingEntry']!;
  }

  String get loadingEntries {
    return _localizedValues[locale.languageCode]!['loadingEntries']!;
  }

  String get mustBeWholeNumber {
    return _localizedValues[locale.languageCode]!['mustBeWholeNumber']!;
  }

  String get noEntriesToDisplay {
    return _localizedValues[locale.languageCode]!['noEntriesToDisplay']!;
  }

  String get notes {
    return _localizedValues[locale.languageCode]!['notes']!;
  }

  String get ok {
    return _localizedValues[locale.languageCode]!['ok']!;
  }

  String get refresh {
    return _localizedValues[locale.languageCode]!['refresh']!;
  }

  String get required {
    return _localizedValues[locale.languageCode]!['required']!;
  }

  String get retry {
    return _localizedValues[locale.languageCode]!['retry']!;
  }

  String get retryCopyEntry {
    return _localizedValues[locale.languageCode]!['retryCopyEntry']!;
  }

  String get retryLoadEntries {
    return _localizedValues[locale.languageCode]!['retryLoadEntries']!;
  }

  String get retryLoadEntry {
    return _localizedValues[locale.languageCode]!['retryLoadEntry']!;
  }

  String get search {
    return _localizedValues[locale.languageCode]!['search']!;
  }

  String get sessionExpired {
    return _localizedValues[locale.languageCode]!['sessionExpired']!;
  }

  String get signIn {
    return _localizedValues[locale.languageCode]!['signIn']!;
  }

  String get signInTitle {
    return _localizedValues[locale.languageCode]!['signInTitle']!;
  }

  String get signInInstructions {
    return _localizedValues[locale.languageCode]!['signInInstructions']!;
  }

  String get signOut {
    return _localizedValues[locale.languageCode]!['signOut']!;
  }

  String get updateEntry {
    return _localizedValues[locale.languageCode]!['updateEntry']!;
  }

  String get updateEntryFailed {
    return _localizedValues[locale.languageCode]!['updateEntryFailed']!;
  }

  String get untitledEntry {
    return _localizedValues[locale.languageCode]!['untitledEntry']!;
  }

  String get welcome {
    return _localizedValues[locale.languageCode]!['welcome']!;
  }
}

/// A delegate for [TextLocalizations].
class TextLocalizationsDelegate extends LocalizationsDelegate<TextLocalizations> {

  const TextLocalizationsDelegate();

  @override
  bool isSupported(Locale locale) =>
      TextLocalizations.languages().contains(locale.languageCode);

  @override
  Future<TextLocalizations> load(Locale locale) {
    // Returning a SynchronousFuture here because an async load operation
    // isn't needed to produce an instance of TextLocalizations.
    return SynchronousFuture<TextLocalizations>(TextLocalizations(locale));
  }

  @override
  bool shouldReload(TextLocalizationsDelegate old) => false;
}
