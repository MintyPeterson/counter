import 'package:counter/pages/add_entry/add_entry_page.dart';
import 'package:counter/services/counter/counter_service.dart';
import 'package:counter/services/counter/requests/entry_new_request.dart';
import 'package:counter/services/counter/requests/entry_view_request.dart';
import 'package:counter/services/counter/responses/entry_view_response.dart';
import 'package:counter/services/secure_storage_service.dart';
import 'package:flutter/foundation.dart';

/// The view model for [AddEntryPage].
class AddEntryViewModel extends ChangeNotifier {

  bool _loadingEntry = true;
  bool get loadingEntry => _loadingEntry;

  bool _addingEntry = false;
  bool get addingEntry => _addingEntry;

  Future<EntryViewResponse>? _entryViewFuture;
  Future<EntryViewResponse>? get entryViewFuture => _entryViewFuture;
  
  final CounterService counterService;
  final SecureStorageService secureStorageService;
  AddEntryViewModel(
    this.counterService,
    this.secureStorageService,
  );

  Future<EntryViewResponse?> loadEntry(EntryViewRequest request) async {
    _loadingEntry = true;
    _entryViewFuture = counterService.entryView(request).then((value) {
      _loadingEntry = false;
      return value;
    }).whenComplete(
      () => notifyListeners()
    );
    return await _entryViewFuture;
  }

  Future<void> addEntry(EntryNewRequest entry) async {
    try {
      _addingEntry = true;
      notifyListeners();
      await counterService.entryNew(entry);
    } finally {
      _addingEntry = false;
      notifyListeners();
    }
  }

  Future<void> signOut() {
    return secureStorageService.deleteAll();
  }
}