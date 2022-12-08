import 'package:counter/pages/sign_in/sign_in_view_model.dart';
import 'package:counter/pages/summary/summary_page.dart';
import 'package:counter/text_localizations.dart';
import 'package:flutter/material.dart';

/// A widget that provides a sign in page.
///
/// The sign in pages allows the user to authenticate.
class SignInPage extends StatelessWidget {

  static const String route = '/';

  final SignInViewModel viewModel;
  const SignInPage(
    this.viewModel, {
    Key? key,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(TextLocalizations.of(context).signInTitle),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            const SizedBox(height: 20),
            Text(
              TextLocalizations.of(context).welcome,
              style: const TextStyle(
                fontWeight: FontWeight.bold,
                fontSize: 22
              ),
            ),
            Text(
              TextLocalizations.of(context).signInInstructions,
              style: const TextStyle(
                fontSize: 18
              ),
            ),
            const SizedBox(height: 20),
            ElevatedButton(
              onPressed: viewModel.signingIn ? null : () async {
                await signIn(context);
              },
              child: Text(TextLocalizations.of(context).signIn),
            ),
            if (viewModel.signingIn) ...const <Widget>[
              SizedBox(height: 32),
              Center(child: CircularProgressIndicator()),
            ],
          ],
        ),
      ),
    );
  }

  Future<void> signIn(context) async {
    await viewModel.signIn();
    await Navigator.of(context)
      .pushNamedAndRemoveUntil(SummaryPage.route, (_) => false);
  }
}