import 'package:counter/pages/add_entry/add_entry_page.dart';
import 'package:counter/pages/edit_entry/edit_entry_page.dart';
import 'package:counter/pages/sign_in/sign_in_page.dart';
import 'package:counter/pages/summary/summary_view_model.dart';
import 'package:counter/services/counter/responses/entry_list_entry_response.dart';
import 'package:counter/services/counter/responses/entry_list_group_response.dart';
import 'package:counter/services/counter/responses/entry_list_response.dart';
import 'package:counter/text_localizations.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

/// A widget that provides a summary page.
///
/// The summary page is the landing page after signing in.
class SummaryPage extends StatefulWidget {

  static const String route = 'summary';

  final SummaryViewModel viewModel;
  const SummaryPage(
    this.viewModel, {
    Key? key,
  }) : super(key: key);

  @override
  State<SummaryPage> createState() => _SummaryPageState();
}

/// Defines the items for the [SummaryPage] overflow menu.
enum SummaryPageOverflowMenu { refresh, signOut }

/// The logic and internal state for [SummaryPage].
class _SummaryPageState extends State<SummaryPage> {

  @override
  void initState() {
    super.initState();
    widget.viewModel.loadInitialEntryList();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(TextLocalizations.of(context).counter),
        actions: <Widget>[
          PopupMenuButton<SummaryPageOverflowMenu>(
            onSelected: (SummaryPageOverflowMenu item) async {
              switch (item) {
                case SummaryPageOverflowMenu.refresh: {
                  await _refreshEntryList();
                }
                break;
                case SummaryPageOverflowMenu.signOut: {
                  await _signOut();
                }
                break;
              }
            },
            itemBuilder: (BuildContext context) =>
              <PopupMenuEntry<SummaryPageOverflowMenu>>[
                PopupMenuItem<SummaryPageOverflowMenu>(
                  value: SummaryPageOverflowMenu.refresh,
                  child: Text(TextLocalizations.of(context).refresh),
                ),
                PopupMenuItem<SummaryPageOverflowMenu>(
                  value: SummaryPageOverflowMenu.signOut,
                  child: Text(TextLocalizations.of(context).signOut),
                ),
              ])
        ],
      ),
      body: FutureBuilder<EntryListResponse>(
        future: widget.viewModel.entryListFuture,
        builder: (_, AsyncSnapshot<EntryListResponse> snapshot) {
          switch (snapshot.connectionState) {
            case ConnectionState.done:
              if (snapshot.hasError) {
                return Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: <Widget>[
                      Text(TextLocalizations.of(context).retryLoadEntries),
                      const SizedBox(height: 20),
                      ElevatedButton(
                        onPressed: () async {
                          await _refreshEntryList();
                        },
                        child: Text(TextLocalizations.of(context).retry),
                      ),
                    ]),
                );
              }
              final List<EntryListGroupResponse> groups =
                snapshot.data?.groups ?? <EntryListGroupResponse>[];
              if (groups.isEmpty) {
                return Center(
                  child: Text(TextLocalizations.of(context).noEntriesToDisplay),
                );
              }
              return ListView.builder(
                itemCount: groups.length,
                itemBuilder: (_, int index) {
                  final EntryListGroupResponse group = groups[index];
                  return Theme(
                    data: Theme.of(context).copyWith(dividerColor: Colors.transparent),
                    child: ExpansionTile(
                      key: PageStorageKey(groups[index].name),
                      controlAffinity: ListTileControlAffinity.leading,
                      title: Row(
                        children: <Widget>[
                          Expanded(
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: <Widget>[
                                Text(group.name)
                              ],
                            ),
                          ),
                          Text('${group.isEstimate ? '~' : ''}${group.total.toString()}'),
                        ],
                      ),
                      children: <Widget>[
                        Column(
                          children: _buildGroupContent(group),
                        ),
                      ],
                    )
                  );
                },
              );
            default:
              return Center(
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  crossAxisAlignment: CrossAxisAlignment.center,
                  children: <Widget>[
                    Text(TextLocalizations.of(context).loadingEntries),
                    const SizedBox(height: 32),
                    const CircularProgressIndicator(),
                  ],
                ),
              );
          }
        },
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () async {
          await _addEntry();
        },
        child: const Icon(Icons.add),
      ),
    );
  }

  _buildGroupContent(EntryListGroupResponse group) {
    List<Widget> columnContent = [];
    for (EntryListEntryResponse entry in group.entries) {
      columnContent.add(
        ListTile(
          title: Row(
            children: <Widget>[
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: <Widget>[
                    (entry.notes == null ? Text(TextLocalizations.of(context).untitledEntry) : Text(entry.notes!))
                  ],
                ),
              ),
              Text('${entry.isEstimate ? '~' : ''}${entry.entry.toString()}'),
            ],
          ),
          onTap: () async {
            await _editEntry(entry.entryId);
          },
        ),
      );
    }
    return columnContent;
  }

  Future<void> _refreshEntryList() async {
    try
    {
      await widget.viewModel.refreshEntryList();
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
      }
    }
  }

  Future<void> _signOut() async {
    await widget.viewModel.signOut();
    if (!mounted) {
      return;
    }
    Navigator.of(context)
      .pushNamedAndRemoveUntil(SignInPage.route, (_) => false);
  }

  Future<void> _addEntry() async {
    final bool? result = (await Navigator.of(context).pushNamed(AddEntryPage.route)) as bool?;
    if (result ?? false) {
      await _refreshEntryList();
    }
  }

  Future<void> _editEntry(String entryId) async {
    final bool? result = (
      await Navigator.of(context).pushNamed(EditEntryPage.route, arguments: entryId)
    ) as bool?;
    if (result ?? false) {
      await _refreshEntryList();
    }
  }
}