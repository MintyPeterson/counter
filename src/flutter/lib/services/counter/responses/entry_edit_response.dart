import 'package:json_annotation/json_annotation.dart';
import 'package:counter/services/counter/counter_service.dart';

part 'entry_edit_response.g.dart';

/// Represents a response from the [CounterService] entry edit endpoint.
@JsonSerializable(createToJson: false)
class EntryEditResponse {

  final String entryId;
  const EntryEditResponse({
    required this.entryId,
  });

  factory EntryEditResponse.fromJson(Map<String, dynamic> json) =>
    _$EntryEditResponseFromJson(json);
}