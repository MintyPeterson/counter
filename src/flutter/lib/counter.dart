import 'package:counter/pages/add_entry/add_entry_page.dart';
import 'package:counter/pages/add_entry/add_entry_view_model.dart';
import 'package:counter/pages/edit_entry/edit_entry_page.dart';
import 'package:counter/pages/edit_entry/edit_entry_view_model.dart';
import 'package:counter/pages/sign_in/sign_in_page.dart';
import 'package:counter/pages/sign_in/sign_in_view_model.dart';
import 'package:counter/pages/summary/summary_page.dart';
import 'package:counter/pages/summary/summary_view_model.dart';
import 'package:counter/text_localizations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:provider/provider.dart';

/// A widget that provides the application.
class Counter extends StatelessWidget {

  final String initialRoute;
  const Counter(
    this.initialRoute, {
    Key? key,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      theme: ThemeData(
        brightness: Brightness.light,
        primarySwatch: Colors.indigo
      ),
      darkTheme: ThemeData(
        brightness: Brightness.dark,
      ),
      themeMode: ThemeMode.system,
      initialRoute: initialRoute,
      onGenerateTitle: (context) => TextLocalizations.of(context).counter,
      onGenerateRoute: (RouteSettings settings) {
        switch (settings.name) {
          case SignInPage.route:
            return MaterialPageRoute(
              builder: (_) => Consumer<SignInViewModel>(
                builder: (_, SignInViewModel viewModel, __) =>
                    SignInPage(viewModel),
              ),
            );
          case SummaryPage.route:
            return MaterialPageRoute(
              builder: (_) => Consumer<SummaryViewModel>(
                builder: (_, SummaryViewModel viewModel, __) =>
                    SummaryPage(viewModel),
              ),
            );
          case AddEntryPage.route:
            return MaterialPageRoute(
              builder: (_) => Consumer<AddEntryViewModel>(
                builder: (_, AddEntryViewModel viewModel, __) =>
                    AddEntryPage(viewModel),
              ),
            );
          case EditEntryPage.route:
            final String entryId = settings.arguments as String;
            return MaterialPageRoute(
              builder: (_) => Consumer<EditEntryViewModel>(
                builder: (_, EditEntryViewModel viewModel, __) =>
                  EditEntryPage(entryId, viewModel),
                ),
              );
        }
        return null;
      },
      localizationsDelegates: const [
        TextLocalizationsDelegate(),
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
      ],
      supportedLocales: const [
        Locale('en', ''), // English
      ],
    );
  }
}