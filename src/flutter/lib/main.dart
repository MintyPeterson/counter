import 'package:counter/counter.dart';
import 'package:counter/pages/add_entry/add_entry_view_model.dart';
import 'package:counter/pages/edit_entry/edit_entry_view_model.dart';
import 'package:counter/pages/sign_in/sign_in_page.dart';
import 'package:counter/pages/sign_in/sign_in_view_model.dart';
import 'package:counter/pages/summary/summary_page.dart';
import 'package:counter/pages/summary/summary_view_model.dart';
import 'package:counter/services/authorization_service.dart';
import 'package:counter/services/counter/counter_service.dart';
import 'package:counter/services/secure_storage_service.dart';
import 'package:flutter/material.dart';
import 'package:flutter_appauth/flutter_appauth.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:provider/provider.dart';
import 'package:provider/single_child_widget.dart';

/// The entry point for the application.
Future<void> main() async {

  WidgetsFlutterBinding.ensureInitialized();

  const FlutterSecureStorage secureStorage = FlutterSecureStorage();
  final SecureStorageService secureStorageService = SecureStorageService(secureStorage);
  final String? refreshToken = await secureStorageService.getRefreshToken();

  final String initialRoute =
    refreshToken == null ? SignInPage.route : SummaryPage.route;

  runApp(
    MultiProvider(
      providers: <SingleChildWidget>[
        Provider<FlutterAppAuth>(
          create: (_) => const FlutterAppAuth(),
        ),
        ProxyProvider<FlutterAppAuth, AuthorizationService>(
          update: (_, FlutterAppAuth appAuth, __) =>
            AuthorizationService(appAuth, secureStorageService),
        ),
        ProxyProvider<AuthorizationService, CounterService>(
          update: (_, AuthorizationService authorizationService, __) =>
            CounterService(authorizationService),
        ),
        ChangeNotifierProvider<SignInViewModel>(
          create: (BuildContext context) => SignInViewModel(
            Provider.of<AuthorizationService>(context, listen: false),
          ),
        ),
        ChangeNotifierProvider<SummaryViewModel>(
          create: (BuildContext context) {
            return SummaryViewModel(
              Provider.of<CounterService>(context, listen: false),
              secureStorageService);
          },
        ),
        ChangeNotifierProvider<AddEntryViewModel>(
          create: (BuildContext context) {
            return AddEntryViewModel(
              Provider.of<CounterService>(context, listen: false),
              secureStorageService);
          },
        ),
        ChangeNotifierProvider<EditEntryViewModel>(
          create: (BuildContext context) {
            return EditEntryViewModel(
              Provider.of<CounterService>(context, listen: false),
              secureStorageService);
          },
        ),
      ],
      child: Counter(initialRoute),
    ),
  );
}
