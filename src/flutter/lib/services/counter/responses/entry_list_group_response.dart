import 'package:counter/services/counter/responses/entry_list_entry_response.dart';
import 'package:counter/services/counter/responses/entry_list_response.dart';

/// Represents a group in a [EntryListResponse].
class EntryListGroupResponse {

  final String name;
  final int total;
  final List<EntryListEntryResponse> entries;
  const EntryListGroupResponse({
    required this.name,
    required this.total,
    required this.entries
  });

  factory EntryListGroupResponse.fromJson(Map<String, dynamic> json) {
    return EntryListGroupResponse(
      name: json['name'] as String,
      total: json['total'] as int,
      entries: List<EntryListEntryResponse>.from(
        json['entries'].map((entry) => EntryListEntryResponse.fromJson(entry)))
    );
  }
}