import 'package:json_annotation/json_annotation.dart';
import 'package:counter/services/counter/counter_service.dart';

part 'entry_delete_request.g.dart';

/// The request model to the [CounterService] entry delete endpoint.
@JsonSerializable(createFactory: false)
class EntryDeleteRequest {

  final String entryId;
  const EntryDeleteRequest({
    required this.entryId,
  });

  Map<String, dynamic> toJson() => _$EntryDeleteRequestToJson(this);
}