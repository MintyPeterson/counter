import 'package:json_annotation/json_annotation.dart';
import 'package:counter/services/counter/counter_service.dart';

part 'entry_list_request.g.dart';

/// The request model to the [CounterService] entry list endpoint.
@JsonSerializable(createFactory: false)
class EntryListRequest {

  final String? notes;
  const EntryListRequest({
    this.notes,
  });

  Map<String, dynamic> toJson() => _$EntryListRequestToJson(this);
}