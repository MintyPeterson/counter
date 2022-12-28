import 'package:json_annotation/json_annotation.dart';
import 'package:counter/services/counter/counter_service.dart';

part 'entry_edit_request.g.dart';

/// The request model to the [CounterService] entry edit endpoint.
@JsonSerializable(createFactory: false)
class EntryEditRequest {

  final String entryId;
  final DateTime entryDate;
  final int entry;
  final String? notes;
  final bool? isEstimate;
  const EntryEditRequest({
    required this.entryId,
    required this.entryDate,
    required this.entry,
    this.notes,
    this.isEstimate,
  });

  Map<String, dynamic> toJson() => _$EntryEditRequestToJson(this);
}