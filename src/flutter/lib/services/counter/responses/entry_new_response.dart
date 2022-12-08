import 'package:json_annotation/json_annotation.dart';
import 'package:counter/services/counter/counter_service.dart';

part 'entry_new_response.g.dart';

/// Represents a response from the [CounterService] entry new endpoint.
@JsonSerializable(createToJson: false)
class EntryNewResponse {

  final String entryId;
  const EntryNewResponse({
    required this.entryId,
  });

  factory EntryNewResponse.fromJson(Map<String, dynamic> json) =>
    _$EntryNewResponseFromJson(json);
}