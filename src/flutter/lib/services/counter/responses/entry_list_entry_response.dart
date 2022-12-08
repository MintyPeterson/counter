import 'package:counter/services/counter/responses/entry_list_response.dart';
import 'package:json_annotation/json_annotation.dart';

part 'entry_list_entry_response.g.dart';

/// Represents an entry in a [EntryListResponse].
@JsonSerializable(createToJson: false)
class EntryListEntryResponse {

  final String entryId;
  final DateTime entryDate;
  final int entry;
  final String? notes;
  const EntryListEntryResponse({
    required this.entryId,
    required this.entryDate,
    required this.entry,
    required this.notes,
  });

  factory EntryListEntryResponse.fromJson(Map<String, dynamic> json) =>
    _$EntryListEntryResponseFromJson(json);
}