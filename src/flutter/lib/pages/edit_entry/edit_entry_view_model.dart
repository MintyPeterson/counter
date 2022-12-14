import 'package:counter/services/counter/counter_service.dart';
import 'package:counter/services/counter/requests/entry_delete_request.dart';
import 'package:counter/services/counter/requests/entry_edit_request.dart';
import 'package:counter/services/counter/requests/entry_view_request.dart';
import 'package:counter/services/counter/responses/entry_view_response.dart';
import 'package:counter/services/secure_storage_service.dart';
import 'package:flutter/cupertino.dart';

class EditEntryViewModel extends ChangeNotifier {

  bool _loadingEntry = true;
  bool get loadingEntry => _loadingEntry;

  bool _editingEntry = false;
  bool get updatingEntry => _editingEntry;

  Future<EntryViewResponse>? _entryViewFuture;
  Future<EntryViewResponse>? get entryViewFuture => _entryViewFuture;

  final CounterService counterService;
  final SecureStorageService secureStorageService;
  EditEntryViewModel(
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

  Future<void> editEntry(EntryEditRequest entry) async {
    try {
      _editingEntry = true;
      notifyListeners();
      await counterService.entryEdit(entry);
    } finally {
      _editingEntry = false;
      notifyListeners();
    }
  }

  Future<void> deleteEntry(EntryDeleteRequest entry) async {
    try {
      _editingEntry = true;
      notifyListeners();
    await counterService.entryDelete(entry);
    } finally {
      _editingEntry = false;
      notifyListeners();
    }
  }

  Future<void> signOut() {
    return secureStorageService.deleteAll();
  }
}