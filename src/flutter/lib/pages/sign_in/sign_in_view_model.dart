import 'package:counter/pages/sign_in/sign_in_page.dart';
import 'package:counter/services/authorization_service.dart';
import 'package:flutter/foundation.dart';

/// The view model for [SignInPage].
class SignInViewModel extends ChangeNotifier {

  bool _signingIn = false;
  bool get signingIn => _signingIn;

  final AuthorizationService authorisationService;
  SignInViewModel(this.authorisationService);

  Future<void> signIn() async {
    try {
      _signingIn = true;
      notifyListeners();
      await authorisationService.authorize();
    } finally {
      _signingIn = false;
      notifyListeners();
    }
  }
}