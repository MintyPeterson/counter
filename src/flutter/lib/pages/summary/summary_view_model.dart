import 'package:counter/pages/summary/summary_page.dart';
import 'package:counter/services/counter/counter_service.dart';
import 'package:counter/services/counter/responses/entry_list_response.dart';
import 'package:counter/services/secure_storage_service.dart';
import 'package:flutter/foundation.dart';

/// The view model for [SummaryPage].
class SummaryViewModel extends ChangeNotifier {

  Future<EntryListResponse>? _entryListFuture;
  Future<EntryListResponse>? get entryListFuture => _entryListFuture;

  final CounterService counterService;
  final SecureStorageService secureStorageService;
  SummaryViewModel(
    this.counterService,
    this.secureStorageService);

  Future<void> loadInitialEntryList() =>
    _entryListFuture = counterService.entryList();

  Future<void> refreshEntryList() async {
    _entryListFuture = counterService.entryList();
    notifyListeners();
    await _entryListFuture;
  }

  Future<void> signOut() {
    return secureStorageService.deleteAll();
  }
}