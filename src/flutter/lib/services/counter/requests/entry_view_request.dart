import 'package:json_annotation/json_annotation.dart';
import 'package:counter/services/counter/counter_service.dart';

part 'entry_view_request.g.dart';

/// The request model to the [CounterService] entry view endpoint.
@JsonSerializable(createFactory: false)
class EntryViewRequest {

  final String entryId;
  const EntryViewRequest({
    required this.entryId,
  });

  Map<String, dynamic> toJson() => _$EntryViewRequestToJson(this);
}