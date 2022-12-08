import 'package:json_annotation/json_annotation.dart';
import 'package:counter/services/counter/counter_service.dart';

part 'entry_delete_response.g.dart';

/// The response model from the [CounterService] entry delete endpoint.
@JsonSerializable(createToJson: false)
class EntryDeleteResponse {

  final String entryId;
  const EntryDeleteResponse({
    required this.entryId,
  });

  factory EntryDeleteResponse.fromJson(Map<String, dynamic> json) =>
    _$EntryDeleteResponseFromJson(json);
}