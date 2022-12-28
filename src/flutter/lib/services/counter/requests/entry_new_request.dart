import 'package:json_annotation/json_annotation.dart';
import 'package:counter/services/counter/counter_service.dart';

part 'entry_new_request.g.dart';

/// The request model to the [CounterService] entry new endpoint.
@JsonSerializable(createFactory: false)
class EntryNewRequest {

  final DateTime entryDate;
  final int entry;
  final String? notes;
  final bool? isEstimate;
  const EntryNewRequest({
    required this.entryDate,
    required this.entry,
    this.notes,
    this.isEstimate,
  });

  Map<String, dynamic> toJson() => _$EntryNewRequestToJson(this);
}