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
      'counter': 'Counter',
      'date': 'Date',
      'entry': 'Entry',
      'loadingEntries': 'Loading your entries...',
      'mustBeWholeNumber': 'Must be a whole number',
      'noEntriesToDisplay': 'There are no entries to display.',
      'notes': 'Notes',
      'ok': 'OK',
      'refresh': 'Refresh',
      'retry': 'Retry',
      'retryInstructions': 'Your entries could not be loaded. Please try again.',
      'required': 'Required',
      'sessionExpired': 'Your session has expired. Please sign in.',
      'signIn': 'Sign in',
      'signInTitle': 'Counter',
      'signInInstructions': 'Sign in to get started.',
      'signOut': 'Sign out',
      'untitledEntry': 'Untitled',
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

  String get counter {
    return _localizedValues[locale.languageCode]!['counter']!;
  }

  String get date {
    return _localizedValues[locale.languageCode]!['date']!;
  }

  String get entry {
    return _localizedValues[locale.languageCode]!['entry']!;
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

  String get retryInstructions {
    return _localizedValues[locale.languageCode]!['retryInstructions']!;
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