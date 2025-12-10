add_executable(day9)
target_sources(day9
    PRIVATE
    day9.cpp)
target_link_libraries(day9 read_file)
target_include_directories(day9 PRIVATE ../utils)
