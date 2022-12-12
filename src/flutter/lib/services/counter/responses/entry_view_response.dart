import 'package:json_annotation/json_annotation.dart';
import 'package:counter/services/counter/counter_service.dart';

part 'entry_view_response.g.dart';

/// The response model from the [CounterService] entry view endpoint.
@JsonSerializable(createToJson: false)
class EntryViewResponse {

  final String entryId;
  final DateTime entryDate;
  final int entry;
  final String? notes;
  const EntryViewResponse({
    required this.entryId,
    required this.entryDate,
    required this.entry,
    required this.notes,
  });

  factory EntryViewResponse.fromJson(Map<String, dynamic> json) =>
    _$EntryViewResponseFromJson(json);
}