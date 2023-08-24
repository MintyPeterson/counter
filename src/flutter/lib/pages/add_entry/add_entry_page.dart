import 'package:counter/pages/add_entry/add_entry_view_model.dart';
import 'package:counter/pages/sign_in/sign_in_page.dart';
import 'package:counter/services/counter/requests/entry_new_request.dart';
import 'package:counter/services/counter/requests/entry_view_request.dart';
import 'package:counter/services/counter/responses/entry_view_response.dart';
import 'package:counter/text_localizations.dart';
import 'package:flutter/material.dart';
import 'package:flutter/scheduler.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';

/// A widget that provides an add entry page.
///
/// The add entry page allows the user to save a new entry.
class AddEntryPage extends StatefulWidget {

  static const String route = 'addItem';

  final String? entryId;
  final AddEntryViewModel viewModel;
  const AddEntryPage(
    this.entryId,
    this.viewModel, {
    Key? key,
  }) : super(key: key);

  @override
  State<AddEntryPage> createState() => _AddEntryPageState();
}

/// The logic and internal state for [AddEntryPage].
class _AddEntryPageState extends State<AddEntryPage> {

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
    if (widget.entryId != null) {
      widget.viewModel.loadEntry(EntryViewRequest(
        entryId: widget.entryId!
      )).then((value) {
        if (value != null) {
          isEstimate = value.isEstimate;
          _entryController.text = value.entry.toString();
          _notesController.text = value.notes ?? '';
        }
      });
    }
    SchedulerBinding.instance.addPostFrameCallback((_) {
      _setEntryDateTextControllerValue();
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
        title: Text(
          widget.entryId == null
            ? TextLocalizations.of(context).addEntry
            : TextLocalizations.of(context).copyEntry
        ),
      ),
      body: _buildBody(),
      floatingActionButton: FloatingActionButton(
        onPressed: widget.viewModel.addingEntry
            ? null
            : () async {
                await _addEntry();
              },
        child: const Icon(Icons.check),
      ),
    );
  }

  Widget? _buildBody() {
    if (widget.entryId == null) {
      return _buildBodyForm();
    } else {
      return FutureBuilder<EntryViewResponse>(
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
                      Text(TextLocalizations.of(context).retryCopyEntry),
                      const SizedBox(height: 20),
                      ElevatedButton(
                        onPressed: () async {
                          await widget.viewModel.loadEntry(EntryViewRequest(
                            entryId: widget.entryId!
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
              return _buildBodyForm();
            default:
              return Center(
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  crossAxisAlignment: CrossAxisAlignment.center,
                  children: <Widget>[
                    Text(TextLocalizations.of(context).copyingEntry),
                    const SizedBox(height: 32),
                    const CircularProgressIndicator(),
                  ],
                ),
              );
          }
        }
      );
    }
  }

  Widget _buildBodyForm() {
    return Padding(
      padding: const EdgeInsets.all(16),
      child: Form(
        key: _formKey,
        child: Column(
          children: <Widget>[
            GestureDetector(
              onTap:() async {
                if (widget.viewModel.addingEntry) {
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
                  enabled: !widget.viewModel.addingEntry,
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
              enabled: !widget.viewModel.addingEntry,
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
              enabled: !widget.viewModel.addingEntry,
              controller: _notesController,
              decoration: InputDecoration(
                labelText: TextLocalizations.of(context).notes
              ),
            ),
            CheckboxListTile(
              enabled: !widget.viewModel.addingEntry,
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
            if (widget.viewModel.addingEntry) ...const <Widget>[
              SizedBox(height: 32),
              CircularProgressIndicator(),
            ]
          ],
        ),
      ),
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

  Future<void> _addEntry() async {
    try {
      if (_formKey.currentState?.validate() == true) {
        final EntryNewRequest item = EntryNewRequest(
          entryDate: entryDate,
          notes: _notesController.text.trim().isEmpty ? null : _notesController.text.trim(),
          entry: int.parse(_entryController.text),
          isEstimate: isEstimate,
        );
        await widget.viewModel.addEntry(item);
        if (!mounted) {
          return;
        }
        Navigator.of(context).pop(true);
      }
    } on Exception catch (error) {
      if (!mounted) {
        return;
      }
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
            content: Text(TextLocalizations.of(context).addEntryFailed),
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