import 'package:counter/pages/edit_entry/edit_entry_view_model.dart';
import 'package:counter/pages/sign_in/sign_in_page.dart';
import 'package:counter/services/counter/requests/entry_delete_request.dart';
import 'package:counter/services/counter/requests/entry_edit_request.dart';
import 'package:counter/services/counter/requests/entry_view_request.dart';
import 'package:counter/services/counter/responses/entry_view_response.dart';
import 'package:counter/text_localizations.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';

/// A widget that provides an edit entry page.
///
/// The edit entry page allows the user to update an existing entry.
class EditEntryPage extends StatefulWidget {

  static const String route = 'editItem';

  final String entryId;
  final EditEntryViewModel viewModel;
  const EditEntryPage(
    this.entryId,
    this.viewModel, {
    Key? key,
  }) : super(key: key);

  @override
  State<EditEntryPage> createState() => _EditEntryPageState();
}

/// The logic and internal state for [EditEntryPage].
class _EditEntryPageState extends State<EditEntryPage> {

  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();

  late TextEditingController _entryDateController;
  late TextEditingController _notesController;
  late TextEditingController _entryController;

  DateTime entryDate = DateTime.now();
  bool isEstimate = false;

  @override
  void initState() {
    super.initState();
    _entryDateController = TextEditingController();
    _notesController = TextEditingController();
    _entryController = TextEditingController();
    widget.viewModel.loadEntry(EntryViewRequest(
      entryId: widget.entryId
    )).then((value) {
      if (value != null) {
        entryDate = value.entryDate;
        isEstimate = value.isEstimate;
        _setEntryDateTextControllerValue();
        _entryController.text = value.entry.toString();
        _notesController.text = value.notes ?? '';
      }
    });
  }

  @override
  void dispose() {
    _entryDateController.dispose();
    _notesController.dispose();
    _entryController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(TextLocalizations.of(context).updateEntry),
        actions: !widget.viewModel.loadingEntry
          ? <Widget>[
            IconButton(
              icon: const Icon(Icons.delete),
              onPressed: () async {
                await _deleteEntry();
              })
            ]
          : null,
      ),
      body: FutureBuilder<EntryViewResponse>(
        future: widget.viewModel.entryViewFuture,
        builder: (_, AsyncSnapshot<EntryViewResponse> snapshot) {
          switch (snapshot.connectionState) {
            case ConnectionState.done:
              if (snapshot.hasError) {
                return Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: <Widget>[
                      Text(TextLocalizations.of(context).retryLoadEntry),
                      const SizedBox(height: 20),
                      ElevatedButton(
                        onPressed: () async {
                          await widget.viewModel.loadEntry(EntryViewRequest(
                            entryId: widget.entryId
                          )).then((value) {
                            if (value != null) {
                              entryDate = value.entryDate;
                              _setEntryDateTextControllerValue();
                              _entryController.text = value.entry.toString();
                              _notesController.text = value.notes ?? '';
                            }
                          });
                        },
                        child: Text(TextLocalizations.of(context).retry),
                      ),
                    ]),
                );
              }
              return Padding(
                padding: const EdgeInsets.all(16),
                child: Form(
                  key: _formKey,
                  child: Column(
                    children: <Widget>[
                      GestureDetector(
                        onTap:() async {
                          if (widget.viewModel.updatingEntry) {
                            return;
                          }
                          final DateTime? selectedDate = await showDatePicker(
                            context: context,
                            initialDate: entryDate,
                            firstDate: DateTime.now().subtract(const Duration(days: 365 * 5)),
                            lastDate: DateTime.now().add(const Duration(days: 365 * 5)),
                          );
                          if (selectedDate != null && selectedDate != entryDate) {
                            entryDate = selectedDate;
                            _setEntryDateTextControllerValue();
                          }
                        },
                        child: AbsorbPointer(
                          child: TextFormField(
                            enabled: !widget.viewModel.updatingEntry,
                            controller: _entryDateController,
                            keyboardType: TextInputType.datetime,
                            decoration: InputDecoration(
                              labelText: TextLocalizations.of(context).date
                            ),
                            validator: (value) => (value == null ? TextLocalizations.of(context).required : null)
                          ),
                        ),
                      ),
                      TextFormField(
                        enabled: !widget.viewModel.updatingEntry,
                        controller: _entryController,
                        keyboardType: const TextInputType.numberWithOptions(decimal: false),
                        decoration: InputDecoration(
                          labelText: TextLocalizations.of(context).entry
                        ),
                        validator: (value) {
                          if (value == null) {
                            return TextLocalizations.of(context).required;
                          }
                          int? entry = int.tryParse(value);
                          if (entry == null) {
                            return TextLocalizations.of(context).mustBeWholeNumber;
                          }
                          return null;
                        },
                      ),
                      TextFormField(
                        enabled: !widget.viewModel.updatingEntry,
                        controller: _notesController,
                        decoration: InputDecoration(
                          labelText: TextLocalizations.of(context).notes
                        ),
                      ),
                      CheckboxListTile(
                        enabled: !widget.viewModel.updatingEntry,
                        controlAffinity: ListTileControlAffinity.leading,
                        contentPadding: const EdgeInsets.fromLTRB(0, 10, 0, 10),
                        value: isEstimate,
                        onChanged: (bool? value) {
                          setState(() {
                            isEstimate = value!;
                          });
                        },
                        title: Text(TextLocalizations.of(context).isEstimate),
                      ),
                      if (widget.viewModel.updatingEntry) ...const <Widget>[
                        SizedBox(height: 32),
                        CircularProgressIndicator(),
                      ]
                    ],
                  ),
                ),
              );
            default:
              return Center(
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  crossAxisAlignment: CrossAxisAlignment.center,
                  children: <Widget>[
                    Text(TextLocalizations.of(context).loadingEntry),
                    const SizedBox(height: 32),
                    const CircularProgressIndicator(),
                  ],
                ),
              );
          }
        },
      ),
      floatingActionButton: !widget.viewModel.loadingEntry
        ? FloatingActionButton(
            onPressed: widget.viewModel.updatingEntry
              ? null
              : () async {
                await _editEntry();
              },
            child: const Icon(Icons.check),
          )
        : null,
    );
  }

  void _setEntryDateTextControllerValue() {
    _entryDateController.text =
      DateFormat.yMMMd(Localizations.localeOf(context).languageCode).format(entryDate);
  }

  Future<void> _signOut() async {
    await widget.viewModel.signOut();
    if (!mounted) {
      return;
    }
    Navigator.of(context).pushNamedAndRemoveUntil(SignInPage.route, (_) => false);
  }

  Future<void> _editEntry() async {
    try {
      if (_formKey.currentState?.validate() == true) {
        final EntryEditRequest item = EntryEditRequest(
          entryId: widget.entryId,
          entryDate: entryDate,
          notes: _notesController.text.trim().isEmpty ? null : _notesController.text.trim(),
          entry: int.parse(_entryController.text),
          isEstimate: isEstimate,
        );
        await widget.viewModel.editEntry(item);
        if (!mounted) {
          return;
        }
        Navigator.of(context).pop(true);
      }
    } on Exception catch (error) {
      if (error is PlatformException && error.message!.contains('invalid_grant')) {
        await showDialog(
          context: context,
          builder: (_) => AlertDialog(
            content: Text(TextLocalizations.of(context).sessionExpired),
            actions: <Widget>[
              TextButton(
                onPressed: () async {
                  await _signOut();
                },
                child: Text(TextLocalizations.of(context).ok),
              )
            ],
          ),
        );
      } else {
        await showDialog(
          context: context,
          builder: (_) => AlertDialog(
            content: Text(TextLocalizations.of(context).updateEntryFailed),
            actions: <Widget>[
              TextButton(
                onPressed: () {
                  Navigator.of(context).pop();
                },
                child: Text(TextLocalizations.of(context).ok),
              )
            ],
          ),
        );
      }
    }
  }

  Future<void> _deleteEntry() async {
    try {
      final EntryDeleteRequest item = EntryDeleteRequest(
        entryId: widget.entryId,
      );
      await widget.viewModel.deleteEntry(item);
      if (!mounted) {
        return;
      }
      Navigator.of(context).pop(true);
    } on Exception catch (error) {
      if (error is PlatformException && error.message!.contains('invalid_grant')) {
        await showDialog(
          context: context,
          builder: (_) => AlertDialog(
            content: Text(TextLocalizations.of(context).sessionExpired),
            actions: <Widget>[
              TextButton(
                onPressed: () async {
                  await _signOut();
                },
                child: Text(TextLocalizations.of(context).ok),
              )
            ],
          ),
        );
      } else {
        await showDialog(
          context: context,
          builder: (_) => AlertDialog(
            content: Text(TextLocalizations.of(context).deleteEntryFailed),
            actions: <Widget>[
              TextButton(
                onPressed: () {
                  Navigator.of(context).pop();
                },
                child: Text(TextLocalizations.of(context).ok),
              )
            ],
          ),
        );
      }
    }
  }
}